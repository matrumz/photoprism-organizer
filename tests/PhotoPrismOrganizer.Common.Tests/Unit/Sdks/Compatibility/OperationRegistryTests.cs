using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrismOrganizer.Common.Tests.Unit.Sdks.Compatibility;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Mimicking non-test classes")]
[Trait("Category", "Unit")]
public class OperationRegistryTests
{
    // Test service classes with various operation configurations
    public class TestServiceV1
    {
        [Operation("get-user", since: "1.0.0", until: "2.0.0", priority: 1)]
        public string GetUserV1(int id) => $"User-V1-{id}";

        [Operation("create-user", since: "1.0.0", priority: 10)]
        public string CreateUser(string name) => $"Created-V1-{name}";

        [Operation("delete-user", since: "1.0.0", until: "1.5.0")]
        public bool DeleteUserV1(int id) => true;

        public string NonOperationMethod() => "not-an-operation";
    }

    public class TestServiceV2
    {
        [Operation("get-user", since: "2.0.0", priority: 2)]
        public string GetUserV2(int id) => $"User-V2-{id}";

        [Operation("get-user", since: "2.1.0", priority: 3)]
        public string GetUserV2Enhanced(int id, bool includeDetails) => $"User-V2-Enhanced-{id}-{includeDetails}";

        [Operation("delete-user", since: "1.5.0", priority: 1)]
        public bool DeleteUserV2(int id) => false;

        [Operation("legacy-operation", until: "1.0.0")]
        public string LegacyOperation() => "legacy";
    }

    public class TestServiceV3
    {
        [Operation("get-user", since: "3.0.0", priority: 5)]
        public string GetUserV3(int id) => $"User-V3-{id}";

        [Operation("bulk-operation", since: "2.5.0")]
        public string BulkOperation(int[] ids) => $"Bulk-{ids.Length}";
    }

    public class TestServiceWithOptionals
    {
        [Operation("optional-test", since: "1.0.0")]
        public string OptionalTest(string required, string optional = "default") => $"{required}-{optional}";
    }

    public class TestServiceWithNullables
    {
        [Operation("nullable-test", since: "1.0.0")]
        public string NullableTest(int id, string? nullableParam) => $"{id}-{nullableParam ?? "null"}";
    }

    [Fact]
    public void Constructor_WithEmptyServices_CreatesEmptyRegistry()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var services = Array.Empty<object>();

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert
        Assert.NotNull(registry);
    }

    [Fact]
    public void Constructor_WithServices_RegistersApplicableOperations()
    {
        // Arrange
        var version = new SemanticVersion(1, 5, 0);
        var services = new object[] { new TestServiceV1(), new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - Should be able to invoke operations applicable to v1.5.0
        var result = registry.Invoke("get-user", new { id = 123 });
        Assert.Equal("User-V1-123", result); // V1 operation should be available
    }

    [Theory]
    [InlineData("1.0.0", "get-user", 123, "User-V1-123")]
    [InlineData("2.0.0", "get-user", 123, "User-V2-123")]
    [InlineData("3.0.0", "get-user", 123, "User-V3-123")]
    public void Invoke_WithVersionSpecificOperations_InvokesCorrectVersion(
        string versionString,
        string operationKey,
        int argument,
        string expected
    )
    {
        // Arrange
        var version = new SemanticVersion(versionString);
        var services = new object[] { new TestServiceV1(), new TestServiceV2(), new TestServiceV3() };
        var registry = new OperationRegistry(version, services);

        // Act
        var result = registry.Invoke(operationKey, new { id = argument });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Invoke_WithMultipleMethodsSamePriority_InvokesByPriorityOrder()
    {
        // Arrange
        var version = new SemanticVersion(2, 1, 0);
        var services = new object[] { new TestServiceV1(), new TestServiceV2() };
        var registry = new OperationRegistry(version, services);

        // Act - Both get-user operations are applicable, should pick highest priority
        var result = registry.Invoke("get-user", new { id = 123, includeDetails = true });

        // Assert - V2Enhanced has priority 3, should be selected
        Assert.Equal("User-V2-Enhanced-123-True", result);
    }

    [Fact]
    public void Invoke_WithParameterMismatch_ReturnsNull()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { new TestServiceV1() };
        var registry = new OperationRegistry(version, services);

        // Act - Missing required parameter
        var result = registry.Invoke("get-user", new { wrongParamName = 123 });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Invoke_WithTypeMismatch_ReturnsNull()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { new TestServiceV1() };
        var registry = new OperationRegistry(version, services);

        // Act - Wrong parameter type (string instead of int)
        var result = registry.Invoke("get-user", new { id = "not-an-int" });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Invoke_WithNonExistentOperation_ReturnsNull()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { new TestServiceV1() };
        var registry = new OperationRegistry(version, services);

        // Act
        var result = registry.Invoke("non-existent-operation", new { id = 123 });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Constructor_WithVersionOutsideRange_ExcludesIncompatibleOperations()
    {
        // Arrange
        var version = new SemanticVersion(0, 9, 0); // Before v1.0.0
        var services = new object[] { new TestServiceV1(), new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - No operations should be available for v0.9.0
        var result = registry.Invoke("get-user", new { id = 123 });
        Assert.Null(result);
    }

    [Fact]
    public void Constructor_WithLegacyOperations_IncludesOnlyApplicableOnes()
    {
        // Arrange
        var version = new SemanticVersion(0, 5, 0); // Old version
        var services = new object[] { new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - Legacy operation should be available
        var result = registry.Invoke("legacy-operation");
        Assert.Equal("legacy", result);
    }

    [Fact]
    public void Constructor_WithVersionTransition_HandlesCorrectly()
    {
        // Arrange - Version 1.5.0 is at the transition point for delete-user
        var version = new SemanticVersion(1, 5, 0);
        var services = new object[] { new TestServiceV1(), new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - Should use V2 delete-user (since: 1.5.0) over V1 (until: 1.5.0)
        var result = registry.Invoke("delete-user", new { id = 123 });
        Assert.Equal(false, result); // V2 returns false, V1 returns true
    }

    [Fact]
    public void Invoke_WithArrayParameters_HandlesCorrectly()
    {
        // Arrange
        var version = new SemanticVersion(2, 5, 0);
        var services = new object[] { new TestServiceV3() };
        var registry = new OperationRegistry(version, services);

        // Act
        var ids = new[] { 1, 2, 3 };
        var result = registry.Invoke("bulk-operation", new { ids = ids });

        // Assert
        Assert.Equal("Bulk-3", result);
    }

    [Fact]
    public void Constructor_FiltersOutMethodsWithoutOperationAttribute()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { new TestServiceV1() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - Non-operation method should not be invocable
        var result = registry.Invoke("NonOperationMethod");
        Assert.Null(result);
    }

    [Fact]
    public void Constructor_WithMultipleServicesOfSameType_UsesLastRegistered()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var service1 = new TestServiceV1();
        var service2 = new TestServiceV1();
        var services = new object[] { service1, service2 };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - Should work with the last registered service instance
        var result = registry.Invoke("get-user", new { id = 123 });
        Assert.Equal("User-V1-123", result);
    }

    [Theory]
    [InlineData("1.0.0", "create-user", "John", "Created-V1-John")]
    [InlineData("2.0.0", "create-user", "Jane", "Created-V1-Jane")]
    public void Invoke_WithStringParameters_WorksCorrectly(
        string versionString,
        string operationKey,
        string name,
        string expected
    )
    {
        // Arrange
        var version = new SemanticVersion(versionString);
        var services = new object[] { new TestServiceV1() };
        var registry = new OperationRegistry(version, services);

        // Act
        var result = registry.Invoke(operationKey, new { name = name });

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_WithComplexVersionConstraints_FiltersCorrectly()
    {
        // Arrange
        var version = new SemanticVersion(1, 2, 0); // Between 1.0.0 and 2.0.0
        var services = new object[] { new TestServiceV1(), new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - Should have V1 operations available but not V2 operations
        var getUserResult = registry.Invoke("get-user", new { id = 123 });
        Assert.Equal("User-V1-123", getUserResult);

        var deleteResult = registry.Invoke("delete-user", new { id = 123 });
        Assert.Equal(true, deleteResult); // Should be V1 method
    }

    [Fact]
    public void Invoke_WithNullArguments_InvokesMethodWithNoParameters()
    {
        // Arrange
        var version = new SemanticVersion(0, 5, 0);
        var services = new object[] { new TestServiceV2() };
        var registry = new OperationRegistry(version, services);

        // Act - Call with no arguments
        var result = registry.Invoke("legacy-operation");

        // Assert
        Assert.Equal("legacy", result);
    }

    [Fact]
    public void Invoke_WithOptionalParameters_HandlesCorrectly()
    {
        // Arrange - Add a test service with optional parameters
        var testService = new TestServiceWithOptionals();
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { testService };
        var registry = new OperationRegistry(version, services);

        // Act - Call without optional parameter
        var result1 = registry.Invoke("optional-test", new { required = "test" });

        // Act - Call with optional parameter
        var result2 = registry.Invoke("optional-test", new { required = "test", optional = "extra" });

        // Assert
        Assert.Equal("test-default", result1);
        Assert.Equal("test-extra", result2);
    }

    [Fact]
    public void Invoke_WithNullableParameters_AcceptsNullValues()
    {
        // Arrange
        var testService = new TestServiceWithNullables();
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { testService };
        var registry = new OperationRegistry(version, services);

        // Act - Call with null value
        var result = registry.Invoke("nullable-test", new { id = 123, nullableParam = (string?)null });

        // Assert
        Assert.Equal("123-null", result);
    }

    [Fact]
    public void Invoke_WithExtraUnusedProperties_InvokesMethodCorrectly()
    {
        // Arrange
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { new TestServiceV1() };
        var registry = new OperationRegistry(version, services);

        // Act - Call with extra properties that don't match any method parameters
        var result = registry.Invoke("get-user", new
        {
            id = 123,                    // Used by method
            extraProperty = "unused",    // Not used by method
            anotherExtra = 999,         // Not used by method
            yetAnother = true           // Not used by method
        });

        // Assert - Should still invoke the method successfully, ignoring extra properties
        Assert.Equal("User-V1-123", result);
    }

    [Fact]
    public void Invoke_WithPartialPropertiesAndExtras_InvokesMethodCorrectly()
    {
        // Arrange
        var testService = new TestServiceWithOptionals();
        var version = new SemanticVersion(1, 0, 0);
        var services = new object[] { testService };
        var registry = new OperationRegistry(version, services);

        // Act - Call with required property, optional property, and extra unused properties
        var result = registry.Invoke("optional-test", new
        {
            required = "test",           // Used by method
            optional = "extra",          // Used by method
            unusedString = "ignored",    // Not used by method
            unusedNumber = 42,          // Not used by method
            unusedBool = false          // Not used by method
        });

        // Assert - Should invoke method with correct parameters, ignoring extras
        Assert.Equal("test-extra", result);
    }

    [Theory]
    [InlineData("1.4.9")] // Just before the until boundary
    [InlineData("1.0.0")] // At the since boundary (inclusive)
    public void Constructor_WithVersionBeforeUntilBoundary_IncludesOperation(string versionString)
    {
        // Arrange - Test versions that should be included (< 1.5.0)
        var version = new SemanticVersion(versionString);
        var services = new object[] { new TestServiceV1() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - V1 delete-user (until: 1.5.0) should be available
        var result = registry.Invoke("delete-user", new { id = 123 });
        Assert.Equal(true, result);
    }

    [Theory]
    [InlineData("1.5.1")] // Just after the until boundary
    [InlineData("2.0.0")] // Well after the boundary
    public void Constructor_WithVersionAfterUntilBoundary_ExcludesOperation(string versionString)
    {
        // Arrange - Test versions that should exclude the operation (> 1.5.0)
        var version = new SemanticVersion(versionString);
        var services = new object[] { new TestServiceV1() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - V1 delete-user (until: 1.5.0) should NOT be available
        var result = registry.Invoke("delete-user", new { id = 123 });
        Assert.Null(result);
    }

    [Theory]
    [InlineData("2.0.1")] // Just after the since boundary
    [InlineData("3.0.0")] // Well after the boundary
    public void Constructor_WithVersionAfterSinceBoundary_IncludesOperation(string versionString)
    {
        // Arrange - Test versions that should include the operation (> 2.0.0)
        var version = new SemanticVersion(versionString);
        var services = new object[] { new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - V2 get-user (since: 2.0.0) should be available
        var result = registry.Invoke("get-user", new { id = 123 });
        Assert.Equal("User-V2-123", result);
    }

    [Theory]
    [InlineData("1.9.9")] // Just before the since boundary
    [InlineData("0.9.0")] // Well before the boundary
    public void Constructor_WithVersionBeforeSinceBoundary_ExcludesOperation(string versionString)
    {
        // Arrange - Test versions that should exclude the operation (< 2.0.0)
        var version = new SemanticVersion(versionString);
        var services = new object[] { new TestServiceV2() };

        // Act
        var registry = new OperationRegistry(version, services);

        // Assert - V2 get-user (since: 2.0.0) should NOT be available
        var result = registry.Invoke("get-user", new { id = 123 });
        Assert.Null(result);
    }

}
