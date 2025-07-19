using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrismOrganizer.Common.Tests.Unit.Sdks.Compatibility;

[Trait("Category", "Unit")]
public class OperationAttributeTests
{
    [Theory]
    [InlineData("test-operation")]
    [InlineData("get-users")]
    [InlineData("create-photo")]
    public void Constructor_WithOperationKey_SetsOperationKey(string operationKey)
    {
        // Arrange & Act
        var attribute = new OperationAttribute(operationKey);

        // Assert
        Assert.Equal(operationKey, attribute.OperationKey);
    }

    [Theory]
    [InlineData("1.0.0")]
    [InlineData("2.1.0")]
    [InlineData("v3.2.1")]
    public void Constructor_WithSince_SetsSince(string since)
    {
        // Arrange & Act
        var attribute = new OperationAttribute("test-op", since: since);

        // Assert
        Assert.Equal(since, attribute.Since);
    }

    [Theory]
    [InlineData("1.0.0")]
    [InlineData("2.1.0")]
    [InlineData("v3.2.1")]
    public void Constructor_WithUntil_SetsUntil(string until)
    {
        // Arrange & Act
        var attribute = new OperationAttribute("test-op", until: until);

        // Assert
        Assert.Equal(until, attribute.Until);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(-1)]
    public void Constructor_WithPriority_SetsPriority(int priority)
    {
        // Arrange & Act
        var attribute = new OperationAttribute("test-op", priority: priority);

        // Assert
        Assert.Equal(priority, attribute.Priority);
    }

    [Fact]
    public void Constructor_WithDefaults_SetsDefaultValues()
    {
        // Arrange & Act
        var attribute = new OperationAttribute("test-op");

        // Assert
        Assert.Equal("test-op", attribute.OperationKey);
        Assert.Null(attribute.Since);
        Assert.Null(attribute.Until);
        Assert.Equal(0, attribute.Priority);
    }

    [Theory]
    [InlineData("1.0.0", null, null, true)]  // No constraints
    [InlineData("2.0.0", "1.0.0", null, true)]  // Since constraint satisfied
    [InlineData("0.9.0", "1.0.0", null, false)]  // Since constraint not satisfied
    [InlineData("1.0.0", null, "2.0.0", true)]  // Until constraint satisfied
    [InlineData("2.1.0", null, "2.0.0", false)]  // Until constraint not satisfied
    [InlineData("1.5.0", "1.0.0", "2.0.0", true)]  // Both constraints satisfied
    [InlineData("0.9.0", "1.0.0", "2.0.0", false)]  // Since constraint not satisfied
    [InlineData("2.1.0", "1.0.0", "2.0.0", false)]  // Until constraint not satisfied
    public void IsApplicable_WithVariousConstraints_ReturnsCorrectResult(
        string currentVersionString,
        string? since,
        string? until,
        bool expected
    )
    {
        // Arrange
        var currentVersion = new SemanticVersion(currentVersionString);
        var attribute = new OperationAttribute("test-op", since: since, until: until);

        // Act
        var result = attribute.IsApplicable(currentVersion);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsApplicable_WithNoConstraints_AlwaysReturnsTrue()
    {
        // Arrange
        var attribute = new OperationAttribute("test-op");
        var versions = new[]
        {
            new SemanticVersion(0, 1, 0),
            new SemanticVersion(1, 0, 0),
            new SemanticVersion(5, 10, 15)
        };

        // Act & Assert
        foreach (var version in versions)
        {
            Assert.True(attribute.IsApplicable(version));
        }
    }

    [Theory]
    [InlineData("v1.0.0", "1.0.0", true)]  // Version prefix handling
    [InlineData("V2.0.0", "2.0.0", true)]  // Case insensitive prefix
    [InlineData("1.0", "1.0.0", true)]     // Partial version
    public void IsApplicable_WithVersionParsing_HandlesVariousFormats(
        string constraintVersion,
        string currentVersionString,
        bool expected
    )
    {
        // Arrange
        var currentVersion = new SemanticVersion(currentVersionString);
        var attribute = new OperationAttribute("test-op", since: constraintVersion);

        // Act
        var result = attribute.IsApplicable(currentVersion);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AttributeUsage_IsCorrectlyConfigured()
    {
        // Arrange
        var attributeType = typeof(OperationAttribute);

        // Act
        var attributeUsage = attributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(attributeUsage);
        Assert.Equal(AttributeTargets.Method, attributeUsage.ValidOn);
        Assert.False(attributeUsage.AllowMultiple);
    }

}
