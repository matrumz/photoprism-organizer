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
        var method = FindBestMethod(operationKey, arguments, preferAsync: false);
        var target = _serviceMap[method.DeclaringType!];
        var result = method.Invoke(target, BuildMethodArguments(method, arguments));

        // If we found an async method but caller wants sync, block and return result
        if (IsAsyncMethod(method) && result is Task task)
        {
            task.GetAwaiter().GetResult();

            // If it's Task<T>, get the Result property
            if (task.GetType().IsGenericType)
            {
                var resultValue = task.GetType().GetProperty("Result")?.GetValue(task);
                // Don't return VoidTaskResult - treat it as null
                return resultValue?.GetType().Name == "VoidTaskResult" ? null : resultValue;
            }

            // For Task (void async), return null
            return null;
        }

        return result;
    }

    public async Task<object?> InvokeAsync(string operationKey, object? arguments = null)
    {
        var method = FindBestMethod(operationKey, arguments, preferAsync: true);
        var target = _serviceMap[method.DeclaringType!];
        var result = method.Invoke(target, BuildMethodArguments(method, arguments));

        // If we found an async method, await it
        if (IsAsyncMethod(method) && result is Task task)
        {
            await task;

            // If it's Task<T>, get the Result property
            if (task.GetType().IsGenericType)
            {
                var resultValue = task.GetType().GetProperty("Result")?.GetValue(task);
                // Don't return VoidTaskResult - treat it as null
                return resultValue?.GetType().Name == "VoidTaskResult" ? null : resultValue;
            }

            // For Task (void async), return null
            return null;
        }

        // If we found a sync method but caller wants async, wrap in Task
        return await Task.FromResult(result);
    }

    private MethodInfo FindBestMethod(string operationKey, object? arguments, bool preferAsync)
    {
        // Check if the operation exists
        if (!_operationMap.TryGetValue(operationKey, out var methods))
            throw new NotImplementedException($"Operation '{operationKey}' is not registered or not available for version {_version}.");

        // Sort by priority descending, then by "since" descending (newer versions first), then by async preference
        var sortedMethods = methods
            .OrderByDescending(m => m.GetCustomAttribute<OperationAttribute>()!.Priority)
            .ThenByDescending(m => m.GetCustomAttribute<OperationAttribute>()!.Since.ParseSemanticVersionOrDefault("0.0.0"))
            .ThenByDescending(m => IsAsyncMethod(m) == preferAsync ? 1 : 0);

        // Extract properties from anonymous object
        Dictionary<string, (PropertyInfo Property, object? Value)> argumentProps = arguments != null
            ? arguments
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => (p, p.GetValue(arguments)))
            : [];

        // Find the first method that can be invoked with the provided arguments
        return sortedMethods.FirstOrDefault(method => CanInvokeMethod(method, argumentProps))
            ?? throw new NotImplementedException($"Operation '{operationKey}' could not be invoked with the provided arguments. No compatible method signature found.");
    }

    private static bool CanInvokeMethod(MethodInfo method, Dictionary<string, (PropertyInfo Property, object? Value)> argumentProps)
    {
        var parameters = method.GetParameters();

        foreach (var param in parameters)
        {
            var paramName = param.Name!;

            if (argumentProps.TryGetValue(paramName, out var argInfo))
            {
                // Anonymous object property found

                var argValue = argInfo.Value;

                // Check if the argument value is compatible with the parameter type
                if (argValue == null)
                {
                    // Null is only acceptable for nullable parameters
                    if (!IsNullableOrOptional(param))
                        return false;
                }
                else if (!param.ParameterType.IsAssignableFrom(argValue.GetType()))
                    return false; // Type mismatch
            }
            else
            {
                // Parameter not found in arguments

                if (!IsNullableOrOptional(param))
                    return false; // Required, non-nullable, parameter missing
            }
        }

        return true;
    }

    private static object?[] BuildMethodArguments(MethodInfo method, object? arguments)
    {
        var parameters = method.GetParameters();
        var args = new object?[parameters.Length];

        // Extract properties from anonymous object
        Dictionary<string, (PropertyInfo Property, object? Value)> argumentProps = arguments != null
            ? arguments
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => (p, p.GetValue(arguments)))
            : [];

        for (int i = 0; i < parameters.Length; i++)
        {
            var param = parameters[i];
            var paramName = param.Name!;

            args[i] = argumentProps.TryGetValue(paramName, out var argInfo)
                ? argInfo.Value
                : param.HasDefaultValue
                ? param.DefaultValue
                : IsNullableOrOptional(param)
                ? null
                : throw new ArgumentException($"Required parameter '{paramName}' is missing from arguments for method '{method.Name}' and this was not caught by the check method.");
        }

        return args;
    }

    private static bool IsAsyncMethod(MethodInfo method) =>
        typeof(Task).IsAssignableFrom(method.ReturnType);

    private static bool IsNullableOrOptional(ParameterInfo param)
    {
        // Check if parameter has a default value (optional parameter)
        if (param.HasDefaultValue)
            return true;

        // Check if parameter is nullable reference type or nullable value type
        if (param.ParameterType.IsValueType)
            return Nullable.GetUnderlyingType(param.ParameterType) != null;

        // For reference types, check nullable annotation context
        var nullabilityInfo = new NullabilityInfoContext().Create(param);
        return nullabilityInfo.ReadState != NullabilityState.NotNull;
    }

}
