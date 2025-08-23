# PhotoPrism SDK - REST API Client

This directory contains the REST API client implementation for the PhotoPrism SDK, providing comprehensive access to PhotoPrism's REST API endpoints with advanced features suitable for production use.

## Features

- **Comprehensive API Coverage**: Full support for Photos, Albums, Labels, and Files endpoints
- **Automatic Retry Logic**: Exponential backoff retry strategy with configurable policies
- **Rate Limiting**: Token bucket algorithm to prevent API throttling
- **Intelligent Caching**: Memory-based caching with TTL support
- **Authentication Management**: Automatic token refresh and session handling
- **Circuit Breaker Pattern**: Prevents cascading failures with circuit breaker protection
- **Operation Attributes**: Version compatibility system using OperationAttribute decorations
- **Robust Error Handling**: Comprehensive exception hierarchy with detailed error information
- **Logging Integration**: Full logging support with configurable verbosity levels

## Architecture

### Core Components

- **RestClientBase**: Abstract base class providing common HTTP functionality
- **RestClientConfiguration**: Centralized configuration with validation
- **RestClientFactory**: Factory pattern for creating configured clients
- **PhotoPrismRestClient**: Unified entry point for all API operations

### Specialized Clients

- **PhotosRestClient**: Photo search, management, and operations
- **AlbumsRestClient**: Album management and photo associations
- **LabelsRestClient**: Label management and search functionality
- **FilesRestClient**: File operations, downloads, and thumbnails

### Infrastructure

- **AuthenticationManager**: Session and token management
- **RateLimiter**: Request throttling with token bucket algorithm
- **CacheManager**: In-memory caching with TTL support
- **RetryPolicy**: Configurable retry logic with exponential backoff
- **ApiException**: Structured error handling with HTTP status codes

## Quick Start

```csharp
// Basic configuration
var config = new RestClientConfiguration
{
    BaseUrl = "https://your-photoprism-instance.com",
    Username = "your-username",
    Password = "your-password",
    EnableCaching = true,
    MaxRetryAttempts = 3
};

// Create client
using var client = new PhotoPrismRestClient(config);

// Search photos
var searchRequest = new PhotoSearchRequest
{
    Query = "sunset",
    Count = 20,
    Offset = 0
};

var photos = await client.Photos.SearchPhotosAsync(searchRequest);

// Get album details
var album = await client.Albums.GetAlbumAsync("album-uid");

// Search labels
var labels = await client.Labels.SearchLabelsAsync(new LabelSearchRequest
{
    Query = "nature",
    Count = 10
});

// Download file
using var fileStream = await client.Files.DownloadFileAsync("file-hash");
```

## Configuration Options

### Authentication
```csharp
// Username/Password authentication
config.Username = "admin";
config.Password = "password";

// Or use access token
config.AccessToken = "your-api-token";
```

### Retry Policy
```csharp
config.MaxRetryAttempts = 3;
config.RetryBaseDelay = TimeSpan.FromSeconds(1);
config.MaxRetryDelay = TimeSpan.FromSeconds(30);
config.RetryableStatusCodes = new[]
{
    HttpStatusCode.RequestTimeout,
    HttpStatusCode.TooManyRequests,
    HttpStatusCode.InternalServerError
};
```

### Rate Limiting
```csharp
config.RateLimitRequestsPerMinute = 100; // Max 100 requests per minute
```

### Caching
```csharp
config.EnableCaching = true;
config.DefaultCacheTtl = TimeSpan.FromMinutes(5);
```

### Circuit Breaker
```csharp
config.CircuitBreakerFailureThreshold = 5;
config.CircuitBreakerOpenDuration = TimeSpan.FromMinutes(1);
```

## Operation Attributes

All API methods are decorated with `[Operation]` attributes for version compatibility:

```csharp
[Operation("photos.search", since: "1.0.0")]
public async Task<PhotoSearchResponse?> SearchPhotosAsync(...)

[Operation("albums.create", since: "1.2.0")]
public async Task<AlbumResponse?> CreateAlbumAsync(...)
```

The operation registry can filter and prioritize methods based on PhotoPrism version compatibility.

## Error Handling

The client provides comprehensive error handling:

```csharp
try
{
    var photos = await client.Photos.SearchPhotosAsync(request);
}
catch (ApiException ex)
{
    // HTTP errors with status codes and response content
    Console.WriteLine($"API Error: {ex.StatusCode} - {ex.Message}");
}
catch (AuthenticationException ex)
{
    // Authentication failures
    Console.WriteLine($"Auth Error: {ex.Message}");
}
catch (RateLimitException ex)
{
    // Rate limiting errors
    Console.WriteLine($"Rate Limited: {ex.Message}");
}
```

## Logging

The client supports comprehensive logging:

```csharp
var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole().SetMinimumLevel(LogLevel.Debug));

var client = new PhotoPrismRestClient(config, loggerFactory: loggerFactory);
```

## Integration with Dependency Injection

```csharp
services.AddSingleton<RestClientConfiguration>(config);
services.AddHttpClient();
services.AddSingleton<RestClientFactory>();
services.AddScoped<PhotoPrismRestClient>();
```

## Thread Safety

All clients are thread-safe and can be used concurrently. The factory pattern ensures proper resource management and configuration isolation.

## Disposal

Always dispose clients to ensure proper cleanup:

```csharp
using var client = new PhotoPrismRestClient(config);
// Client will be properly disposed automatically
```

## Performance Considerations

- **Caching**: Enable caching for frequently accessed data
- **Rate Limiting**: Configure appropriate limits to avoid throttling
- **Connection Pooling**: Use IHttpClientFactory for connection reuse
- **Retry Logic**: Configure retry policies based on your requirements
- **Circuit Breaker**: Prevents resource exhaustion during outages

## API Compatibility

The REST client is designed to work with PhotoPrism v1.0.0+ and uses operation attributes to ensure compatibility across different PhotoPrism versions.
