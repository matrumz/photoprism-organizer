using PhotoPrismOrganizer.Common.Extensions;

namespace PhotoPrismOrganizer.Common.Tests.Unit.Extensions;

public class ObjectExtensionsTests
{

    [Fact]
    public void ToKeyValuePairs_WithNullObject_ReturnsEmpty()
    {
        // Arrange
        object? obj = null;

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToKeyValuePairs_WithSimpleProperties_ReturnsCorrectPairs()
    {
        // Arrange
        var obj = new
        {
            StringProperty = "test",
            IntProperty = 42,
            NullableBoolProperty = (bool?)true
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        Assert.Contains(result, kvp => kvp.Key == "StringProperty" && kvp.Value == "test");
        Assert.Contains(result, kvp => kvp.Key == "IntProperty" && kvp.Value == "42");
        Assert.Contains(result, kvp => kvp.Key == "NullableBoolProperty" && kvp.Value == "True");
    }

    [Fact]
    public void ToKeyValuePairs_WithArrayProperty_ReturnsMultiplePairsWithSameKey()
    {
        // Arrange
        var obj = new
        {
            ArrayProperty = new[] { "value1", "value2", "value3" }
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        var arrayPairs = result.Where(kvp => kvp.Key == "ArrayProperty").ToList();
        Assert.Equal(3, arrayPairs.Count);
        Assert.Contains(arrayPairs, kvp => kvp.Value == "value1");
        Assert.Contains(arrayPairs, kvp => kvp.Value == "value2");
        Assert.Contains(arrayPairs, kvp => kvp.Value == "value3");
    }

    [Fact]
    public void ToKeyValuePairs_WithEmptyArray_ReturnsNoPairs()
    {
        // Arrange
        var obj = new
        {
            ArrayProperty = Array.Empty<string>()
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        var arrayPairs = result.Where(kvp => kvp.Key == "ArrayProperty").ToList();
        Assert.Empty(arrayPairs);
    }

    [Fact]
    public void ToKeyValuePairs_WithList_ReturnsMultiplePairsWithSameKey()
    {
        // Arrange
        var obj = new
        {
            ListProperty = new List<string> { "item1", "item2" }
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        var listPairs = result.Where(kvp => kvp.Key == "ListProperty").ToList();
        Assert.Equal(2, listPairs.Count);
        Assert.Contains(listPairs, kvp => kvp.Value == "item1");
        Assert.Contains(listPairs, kvp => kvp.Value == "item2");
    }

    [Fact]
    public void ToKeyValuePairs_WithEmptyList_ReturnsNoPairs()
    {
        // Arrange
        var obj = new
        {
            ListProperty = new List<string>()
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        var listPairs = result.Where(kvp => kvp.Key == "ListProperty").ToList();
        Assert.Empty(listPairs);
    }

    [Fact]
    public void ToKeyValuePairs_WithIntArray_ConvertsToStrings()
    {
        // Arrange
        var obj = new
        {
            IntArrayProperty = new[] { 1, 2, 3 }
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        var intArrayPairs = result.Where(kvp => kvp.Key == "IntArrayProperty").ToList();
        Assert.Equal(3, intArrayPairs.Count);
        Assert.Contains(intArrayPairs, kvp => kvp.Value == "1");
        Assert.Contains(intArrayPairs, kvp => kvp.Value == "2");
        Assert.Contains(intArrayPairs, kvp => kvp.Value == "3");
    }

    [Fact]
    public void ToKeyValuePairs_WithNullProperty_ExcludesNullValuesByDefault()
    {
        // Arrange
        var obj = new
        {
            StringProperty = (string?)null, // This is null
            NullableBoolProperty = (bool?)null, // This is null
            IntProperty = 42
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert - null values are excluded by default
        Assert.DoesNotContain(result, kvp => kvp.Key == "StringProperty");
        Assert.DoesNotContain(result, kvp => kvp.Key == "NullableBoolProperty");
        Assert.Contains(result, kvp => kvp.Key == "IntProperty" && kvp.Value == "42");
    }

    [Fact]
    public void ToKeyValuePairs_WithEmptyStringProperty_IncludesEmptyString()
    {
        // Arrange
        var obj = new
        {
            StringProperty = "", // This is empty string, not null
            IntProperty = 42
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert - empty strings are included
        Assert.Contains(result, kvp => kvp.Key == "StringProperty" && kvp.Value == string.Empty);
        Assert.Contains(result, kvp => kvp.Key == "IntProperty" && kvp.Value == "42");
    }

    [Fact]
    public void ToKeyValuePairs_WithIncludeNullValues_IncludesNullProperties()
    {
        // Arrange
        var obj = new
        {
            StringProperty = (string?)null,
            NullableBoolProperty = (bool?)null,
            IntProperty = 42
        };

        // Act
        var result = obj.ToKeyValuePairs(includeNullValues: true).ToList();

        // Assert
        Assert.Contains(result, kvp => kvp.Key == "StringProperty" && kvp.Value == null);
        Assert.Contains(result, kvp => kvp.Key == "NullableBoolProperty" && kvp.Value == null);
        Assert.Contains(result, kvp => kvp.Key == "IntProperty" && kvp.Value == "42");
    }

    [Fact]
    public void ToKeyValuePairs_WithKeyFormatter_AppliesFormatter()
    {
        // Arrange
        var obj = new
        {
            StringProperty = "test",
            IntProperty = 42
        };

        // Act - Convert to lowercase
        var result = obj.ToKeyValuePairs(includeNullValues: false, keyFormatter: key => key.ToLowerInvariant()).ToList();

        // Assert
        Assert.Contains(result, kvp => kvp.Key == "stringproperty" && kvp.Value == "test");
        Assert.Contains(result, kvp => kvp.Key == "intproperty" && kvp.Value == "42");
    }

    [Fact]
    public void ToKeyValuePairs_WithSnakeCaseFormatter_ConvertsCorrectly()
    {
        // Arrange
        var obj = new
        {
            StringProperty = "test",
            NullableBoolProperty = (bool?)true
        };

        // Act - Convert PascalCase to snake_case
        var result = obj.ToKeyValuePairs(
            includeNullValues: false,
            keyFormatter: key => string.Concat(key.Select((c, i) => i > 0 && char.IsUpper(c) ? $"_{char.ToLower(c)}" : char.ToLower(c).ToString()))
        ).ToList();

        // Assert
        Assert.Contains(result, kvp => kvp.Key == "string_property" && kvp.Value == "test");
        Assert.Contains(result, kvp => kvp.Key == "nullable_bool_property" && kvp.Value == "True");
    }

    [Fact]
    public void ToKeyValuePairs_WithStringProperty_DoesNotTreatAsCharArray()
    {
        // Arrange
        var obj = new
        {
            StringProperty = "hello"
        };

        // Act
        var result = obj.ToKeyValuePairs().ToList();

        // Assert
        var stringPairs = result.Where(kvp => kvp.Key == "StringProperty").ToList();
        Assert.Single(stringPairs);
        Assert.Equal("hello", stringPairs[0].Value);
    }
}
