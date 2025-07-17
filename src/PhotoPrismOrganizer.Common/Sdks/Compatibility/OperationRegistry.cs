using System.Reflection;

namespace PhotoPrismOrganizer.Common.Sdks.Compatibility;

/// <summary>
/// Registry for SDK methods, i.e. "operations", that can be invoked based on their compatibility with the current version.
/// </summary>
public class OperationRegistry
{
    private readonly SemanticVersion _version;
    private readonly Dictionary<string, List<MethodInfo>> _operationMap = [];
    private readonly Dictionary<Type, object> _serviceMap = [];

    public OperationRegistry(SemanticVersion version, IEnumerable<object> services)
    {
        _version = version;

        foreach (var service in services)
        {
            _serviceMap[service.GetType()] = service;

            service.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(method => method.GetCustomAttribute<OperationAttribute>() != null)
                .Where(method =>
                {
                    var attr = method.GetCustomAttribute<OperationAttribute>()!;
                    return attr.Since.IsValidSemanticVersionOrNull() && attr.Until.IsValidSemanticVersionOrNull();
                })
                .Where(method => method.GetCustomAttribute<OperationAttribute>()!.IsApplicable(_version))
                .ToList()
                .ForEach(method =>
                {
                    var attr = method.GetCustomAttribute<OperationAttribute>()!;
                    if (!_operationMap.ContainsKey(attr.OperationKey))
                        _operationMap[attr.OperationKey] = [];
                    _operationMap[attr.OperationKey].Add(method);
                });
        }
    }

    public object? Invoke(string operationKey, object? arguments = null)
    {
        // Check if the operation exists
        if (!_operationMap.TryGetValue(operationKey, out var methods))
            return null;

        // Sort by priority descending, then by "since" descending (newer versions first)
        var sortedMethods = methods
            .OrderByDescending(m => m.GetCustomAttribute<OperationAttribute>()!.Priority)
            .ThenByDescending(m => m.GetCustomAttribute<OperationAttribute>()!.Since.ParseSemanticVersionOrDefault("0.0.0"));

        // Extract properties from anonymous object
        Dictionary<string, (PropertyInfo Property, object? Value)> argumentProps = arguments != null
            ? (Dictionary<string, (PropertyInfo Property, object? Value)>)arguments
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => (p, p.GetValue(arguments)))
            : [];

        // Iterate through sorted methods and try to invoke them with the provided arguments
        foreach (var method in sortedMethods)
        {
            var parameters = method.GetParameters();
            var args = new object?[parameters.Length];
            var canInvoke = true;

            foreach (var (param, i) in parameters.Select((p, index) => (p, index)))
            {
                var paramName = param.Name!;

                if (argumentProps.TryGetValue(paramName, out var argInfo))
                {
                    // Anonymous object property found

                    var argValue = argInfo.Value;

                    // Check if the argument value is compatible with the parameter type
                    if (argValue == null)
                    {
                        // Null is only acceptable for nullable or optional parameters
                        if (!IsNullableOrOptional(param))
                        {
                            canInvoke = false;
                            break;
                        }
                        args[i] = null;
                    }
                    else if (param.ParameterType.IsAssignableFrom(argValue.GetType()))
                    {
                        args[i] = argValue;
                    }
                    else
                    {
                        // Type mismatch
                        canInvoke = false;
                        break;
                    }
                }
                else
                {
                    // Parameter not found in arguments

                    if (param.HasDefaultValue)
                    {
                        args[i] = param.DefaultValue;
                    }
                    else if (IsNullableOrOptional(param))
                    {
                        args[i] = null;
                    }
                    else
                    {
                        // Required parameter missing
                        canInvoke = false;
                        break;
                    }
                }
            }

            if (canInvoke)
            {
                var target = _serviceMap[method.DeclaringType!];
                return method.Invoke(target, args);
            }
        }

        return null;
    }

    private static bool IsNullableOrOptional(ParameterInfo param)
    {
        // Check if parameter is nullable reference type or nullable value type
        if (param.ParameterType.IsValueType)
            return Nullable.GetUnderlyingType(param.ParameterType) != null;
        // For reference types, check nullable annotation context
        var nullabilityInfo = new NullabilityInfoContext().Create(param);
        return nullabilityInfo.ReadState != NullabilityState.NotNull;
    }

}
