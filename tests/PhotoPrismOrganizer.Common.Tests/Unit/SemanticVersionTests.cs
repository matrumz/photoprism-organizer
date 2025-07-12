namespace PhotoPrismOrganizer.Common.Tests.Unit;

[Trait("Category", "Unit")]
public class SemanticVersionTests
{

    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(2, 1, 0)]
    [InlineData(3, 2, 1)]
    [InlineData(10, 20, 30)]
    public void Constructor_WithValidParameters_CreatesSemanticVersion(int major, int minor, int patch)
    {
        // Arrange & Act
        var version = new SemanticVersion(major, minor, patch);

        // Assert
        Assert.Equal(major, version.Major);
        Assert.Equal(minor, version.Minor);
        Assert.Equal(patch, version.Patch);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Constructor_WithOnlyMajor_DefaultsMinorAndPatchToZero(int major)
    {
        // Arrange & Act
        var version = new SemanticVersion(major);

        // Assert
        Assert.Equal(major, version.Major);
        Assert.Equal(0, version.Minor);
        Assert.Equal(0, version.Patch);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(5, 3)]
    [InlineData(10, 15)]
    public void Constructor_WithMajorAndMinor_DefaultsPatchToZero(int major, int minor)
    {
        // Arrange & Act
        var version = new SemanticVersion(major, minor);

        // Assert
        Assert.Equal(major, version.Major);
        Assert.Equal(minor, version.Minor);
        Assert.Equal(0, version.Patch);
    }

    [Theory]
    [InlineData("1.0.0", 1, 0, 0)]
    [InlineData("2.1.0", 2, 1, 0)]
    [InlineData("3.2.1", 3, 2, 1)]
    [InlineData("10.20.30", 10, 20, 30)]
    public void Constructor_WithValidVersionString_ParsesCorrectly(string versionString, int expectedMajor, int expectedMinor, int expectedPatch)
    {
        // Arrange & Act
        var version = new SemanticVersion(versionString);

        // Assert
        Assert.Equal(expectedMajor, version.Major);
        Assert.Equal(expectedMinor, version.Minor);
        Assert.Equal(expectedPatch, version.Patch);
    }

    [Theory]
    [InlineData("v1.0.0", 1, 0, 0)]
    [InlineData("V2.1.0", 2, 1, 0)]
    [InlineData("v3.2.1", 3, 2, 1)]
    public void Constructor_WithVersionPrefix_ParsesCorrectly(string versionString, int expectedMajor, int expectedMinor, int expectedPatch)
    {
        // Arrange & Act
        var version = new SemanticVersion(versionString);

        // Assert
        Assert.Equal(expectedMajor, version.Major);
        Assert.Equal(expectedMinor, version.Minor);
        Assert.Equal(expectedPatch, version.Patch);
    }

    [Theory]
    [InlineData("1.0.0", 1, 0, 0)]
    [InlineData("2.1.0", 2, 1, 0)]
    [InlineData("3.2.1", 3, 2, 1)]
    [InlineData("v1.0.0", 1, 0, 0)]
    [InlineData("V2.1", 2, 1, 0)]
    [InlineData("3", 3, 0, 0)]
    public void Parse_WithValidVersionString_ReturnsCorrectSemanticVersion(string versionString, int expectedMajor, int expectedMinor, int expectedPatch)
    {
        // Arrange & Act
        var version = SemanticVersion.Parse(versionString);

        // Assert
        Assert.Equal(expectedMajor, version.Major);
        Assert.Equal(expectedMinor, version.Minor);
        Assert.Equal(expectedPatch, version.Patch);
    }

    [Theory]
    [InlineData("1", 1, 0, 0)]
    [InlineData("2.1", 2, 1, 0)]
    [InlineData("v1", 1, 0, 0)]
    [InlineData("V2.1", 2, 1, 0)]
    public void Parse_WithPartialVersionString_DefaultsMissingParts(string versionString, int expectedMajor, int expectedMinor, int expectedPatch)
    {
        // Arrange & Act
        var version = SemanticVersion.Parse(versionString);

        // Assert
        Assert.Equal(expectedMajor, version.Major);
        Assert.Equal(expectedMinor, version.Minor);
        Assert.Equal(expectedPatch, version.Patch);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Parse_WithNullOrEmptyString_ThrowsArgumentException(string? versionString)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(versionString!));
    }

    [Theory]
    [InlineData("1.0.0", "1.0.0", 0)]
    [InlineData("2.0.0", "1.0.0", 1)]
    [InlineData("1.0.0", "2.0.0", -1)]
    [InlineData("1.1.0", "1.0.0", 1)]
    [InlineData("1.0.0", "1.1.0", -1)]
    [InlineData("1.0.1", "1.0.0", 1)]
    [InlineData("1.0.0", "1.0.1", -1)]
    [InlineData("2.1.3", "2.1.2", 1)]
    [InlineData("2.1.2", "2.1.3", -1)]
    public void CompareTo_WithVariousVersions_ReturnsCorrectComparison(string version1String, string version2String, int expectedResult)
    {
        // Arrange
        var version1 = new SemanticVersion(version1String);
        var version2 = new SemanticVersion(version2String);

        // Act
        var result = version1.CompareTo(version2);

        // Assert
        Assert.Equal(expectedResult, Math.Sign(result));
    }

    [Fact]
    public void CompareTo_WithNullVersion_ReturnsOne()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);

        // Act
        var result = version.CompareTo(null);

        // Assert
        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData("2.0.0", "1.0.0", true)]
    [InlineData("1.1.0", "1.0.0", true)]
    [InlineData("1.0.1", "1.0.0", true)]
    [InlineData("1.0.0", "1.0.0", false)]
    [InlineData("1.0.0", "2.0.0", false)]
    public void GreaterThanOperator_WithVariousVersions_ReturnsCorrectResult(string version1String, string version2String, bool expected)
    {
        // Arrange
        var version1 = new SemanticVersion(version1String);
        var version2 = new SemanticVersion(version2String);

        // Act
        var result = version1 > version2;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2.0.0", "1.0.0", true)]
    [InlineData("1.1.0", "1.0.0", true)]
    [InlineData("1.0.1", "1.0.0", true)]
    [InlineData("1.0.0", "1.0.0", true)]
    [InlineData("1.0.0", "2.0.0", false)]
    public void GreaterThanOrEqualOperator_WithVariousVersions_ReturnsCorrectResult(string version1String, string version2String, bool expected)
    {
        // Arrange
        var version1 = new SemanticVersion(version1String);
        var version2 = new SemanticVersion(version2String);

        // Act
        var result = version1 >= version2;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1.0.0", "2.0.0", true)]
    [InlineData("1.0.0", "1.1.0", true)]
    [InlineData("1.0.0", "1.0.1", true)]
    [InlineData("1.0.0", "1.0.0", false)]
    [InlineData("2.0.0", "1.0.0", false)]
    public void LessThanOperator_WithVariousVersions_ReturnsCorrectResult(string version1String, string version2String, bool expected)
    {
        // Arrange
        var version1 = new SemanticVersion(version1String);
        var version2 = new SemanticVersion(version2String);

        // Act
        var result = version1 < version2;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1.0.0", "2.0.0", true)]
    [InlineData("1.0.0", "1.1.0", true)]
    [InlineData("1.0.0", "1.0.1", true)]
    [InlineData("1.0.0", "1.0.0", true)]
    [InlineData("2.0.0", "1.0.0", false)]
    public void LessThanOrEqualOperator_WithVariousVersions_ReturnsCorrectResult(string version1String, string version2String, bool expected)
    {
        // Arrange
        var version1 = new SemanticVersion(version1String);
        var version2 = new SemanticVersion(version2String);

        // Act
        var result = version1 <= version2;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1.0.0", "1.0.0", true)]
    [InlineData("2.1.3", "2.1.3", true)]
    [InlineData("1.0.0", "1.0.1", false)]
    [InlineData("1.0.0", "2.0.0", false)]
    public void Equality_WithVariousVersions_ReturnsCorrectResult(string version1String, string version2String, bool expected)
    {
        // Arrange
        var version1 = new SemanticVersion(version1String);
        var version2 = new SemanticVersion(version2String);

        // Act
        var result = version1.Equals(version2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var version = new SemanticVersion(1, 2, 3);

        // Act
        var result = version.ToString();

        // Assert
        Assert.Equal("SemanticVersion { Major = 1, Minor = 2, Patch = 3 }", result);
    }

    [Fact]
    public void GetHashCode_ForEqualVersions_ReturnsSameHashCode()
    {
        // Arrange
        var version1 = new SemanticVersion(1, 2, 3);
        var version2 = new SemanticVersion(1, 2, 3);

        // Act
        var hash1 = version1.GetHashCode();
        var hash2 = version2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

}
