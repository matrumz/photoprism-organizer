# SDK[DK]: A Development Kit for Development Kits

This namespace contains resources and tools for building and managing SDKs (Software Development Kits) in a structured and efficient manner.

## Compatibility

The `Compatibility` namespace provides the core infrastructure for version-aware operation management. It enables SDK developers to create multiple implementations of the same operation for different API versions and automatically select the most appropriate one at runtime, allowing for seamless version transitions and backwards compatibility.

### Key Features

- **Declarative Operation Registration**: Use attributes to define operations and their version compatibility
- **Dynamic Operation Invocation**: Call operations using anonymous objects with named parameters
- **Semantic Versioning Support**: Built-in support for semantic versioning with range-based compatibility
- **Priority-based Operation Selection**: Handle multiple implementations with configurable priority levels
- **Type-safe Parameter Matching**: Automatic parameter mapping with type checking and nullable support

### Core Components

#### OperationAttribute

A declarative attribute for marking methods as SDK operations with version constraints.

```csharp
[Operation("get-user", since: "1.0.0", until: "2.0.0", priority: 1)]
public User GetUserV1(int id) => // Implementation for v1.0.0 - v2.0.0

[Operation("get-user", since: "2.0.0", priority: 2)]
public User GetUserV2(int id, bool includeMetadata = false) => // Implementation for v2.0.0+
```

**Parameters:**
- `operationKey` (required): Unique identifier for the operation
- `since` (optional): Minimum version where this operation is available
- `until` (optional): Maximum version where this operation is available (exclusive)
- `priority` (optional): Priority level for operation selection (higher = preferred)

#### OperationRegistry

The central registry that manages and invokes SDK operations based on version compatibility.

```csharp
// Initialize with target version and service instances
var version = new SemanticVersion("2.1.0");
var services = new object[] { new UserServiceV1(), new UserServiceV2() };
var registry = new OperationRegistry(version, services);

// Invoke operations using anonymous objects
var user = registry.Invoke("get-user", new { id = 123, includeMetadata = true });
```

**Key Features:**
- **Automatic Operation Discovery**: Scans service instances for `[Operation]` attributed methods
- **Version Filtering**: Only registers operations compatible with the target version
- **Smart Parameter Matching**: Maps anonymous object properties to method parameters by name
- **Priority-based Selection**: Chooses the highest priority operation when multiple match
- **Type Safety**: Validates parameter types and nullability constraints

### Operation Invocation

The registry supports flexible parameter passing using anonymous objects, making it easy to call operations without knowing their exact signatures:

```csharp
// Simple operation with single parameter
var result = registry.Invoke("delete-user", new { id = 123 });

// Complex operation with multiple parameters
var users = registry.Invoke("search-users", new {
    query = "john",
    maxResults = 10,
    includeInactive = false
});

// Operation with optional parameters
var user = registry.Invoke("get-user", new { id = 123 }); // Uses defaults
var userWithMeta = registry.Invoke("get-user", new { id = 123, includeMetadata = true });

// Operation with no parameters
var status = registry.Invoke("health-check");
```

### Parameter Matching Rules

The registry follows these rules when matching parameters:

1. **Required Parameters**: Must be present in the anonymous object with compatible types
2. **Optional Parameters**: Use default values if not provided in the anonymous object
3. **Nullable Parameters**: Accept `null` values explicitly provided in the anonymous object
4. **Missing Parameters**: Operations fail if required parameters are missing
5. **Type Compatibility**: Parameters must be assignable to the expected types

### Version Constraints

Operations can specify version ranges using semantic versioning:

```csharp
[Operation("legacy-feature", until: "2.0.0")]        // Available in < v2.0.0
[Operation("current-feature", since: "1.5.0")]       // Available in >= v1.5.0
[Operation("specific-range", since: "1.0.0", until: "3.0.0")] // Available in v1.0.0 - v2.x.x
```

Invalid version strings are automatically filtered out during registration, ensuring only properly versioned operations are available.

### Priority System

When multiple operations match the same key and version, priority determines selection:

```csharp
[Operation("get-data", since: "1.0.0", priority: 1)]  // Lower priority
public Data GetDataBasic() => // Basic implementation

[Operation("get-data", since: "1.0.0", priority: 10)] // Higher priority
public Data GetDataOptimized() => // Optimized implementation (selected)
```

Operations with the same priority are further sorted by version (newer preferred).

### Example Usage

```csharp
public class PhotoServiceV1
{
    [Operation("get-photo", since: "1.0.0", until: "2.0.0")]
    public Photo GetPhoto(string id) => new Photo { Id = id, Version = "v1" };

    [Operation("list-photos", since: "1.0.0", priority: 1)]
    public Photo[] ListPhotos(int count = 10) => new Photo[count];
}

public class PhotoServiceV2
{
    [Operation("get-photo", since: "2.0.0", priority: 2)]
    public Photo GetPhoto(string id, bool includeMetadata = false) =>
        new Photo { Id = id, Version = "v2", HasMetadata = includeMetadata };

    [Operation("list-photos", since: "2.0.0", priority: 2)]
    public Photo[] ListPhotos(int count = 10, string? filter = null) =>
        new Photo[count]; // With filtering support
}

// Usage
var version = new SemanticVersion("2.1.0");
var registry = new OperationRegistry(version, new object[] {
    new PhotoServiceV1(),
    new PhotoServiceV2()
});

// Calls PhotoServiceV2.GetPhoto (higher version/priority)
var photo = registry.Invoke("get-photo", new { id = "photo123", includeMetadata = true });

// Calls PhotoServiceV2.ListPhotos (higher version/priority)
var photos = registry.Invoke("list-photos", new { count = 5, filter = "landscape" });
```

## Future Extensibility

The SDK namespace is designed to accommodate additional features and namespaces as requirements evolve. Future additions may include:

- **Authentication**: Standardized authentication and authorization patterns
- **Caching**: Intelligent caching strategies for SDK operations
- **Monitoring**: Built-in telemetry and performance monitoring
- **Serialization**: Consistent data serialization and deserialization
- **Configuration**: Dynamic configuration management for SDK behavior

Each new namespace will follow similar patterns of declarative configuration and runtime flexibility while maintaining compatibility with existing components.


