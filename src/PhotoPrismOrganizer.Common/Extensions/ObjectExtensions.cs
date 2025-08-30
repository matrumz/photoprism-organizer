using System.Collections;
using System.Reflection;

namespace PhotoPrismOrganizer.Common.Extensions;

public static class ObjectExtensions
{

    /// <summary>
    /// Converts an object to an enumerable of key-value pairs.
    /// Arrays and collections yield multiple pairs with the same key.
    /// </summary>
    /// <param name="obj">The object to convert.</param>
    /// <param name="includeNullValues">Whether to include properties with null values. Default is false.</param>
    /// <param name="keyFormatter">Function to format property names (e.g., camelCase, snake_case).</param>
    /// <returns>An enumerable of key-value pairs representing the object's properties.</returns>
    public static IEnumerable<KeyValuePair<string, string?>> ToKeyValuePairs(
        this object? obj,
        bool includeNullValues = false,
        Func<string, string>? keyFormatter = null
    )
    {
        if (obj == null)
            yield break;

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead)
                continue;

            var value = property.GetValue(obj);
            var key = keyFormatter != null ? keyFormatter(property.Name) : property.Name;

            foreach (var kvp in ProcessValue(key, value, includeNullValues))
            {
                yield return kvp;
            }
        }
    }

    private static IEnumerable<KeyValuePair<string, string?>> ProcessValue(string key, object? value, bool includeNullValues)
    {
        // Handle null values
        if (value == null)
        {
            if (includeNullValues)
            {
                yield return new(key, null);
            }
            yield break;
        }

        // Handle strings specially to avoid treating them as IEnumerable<char>
        if (value is string stringValue)
        {
            yield return new(key, stringValue);
            yield break;
        }

        // Handle arrays and collections
        if (value is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                var itemValue = item?.ToString() ?? string.Empty;
                yield return new(key, itemValue);
            }
            // Empty collections yield no pairs
            yield break;
        }

        // Handle primitive types and objects
        yield return new(key, value.ToString() ?? string.Empty);
    }
}
