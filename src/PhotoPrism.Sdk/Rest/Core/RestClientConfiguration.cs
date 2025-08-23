using System.Net;

namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// Configuration settings for PhotoPrism REST API clients.
/// </summary>
public class RestClientConfiguration
{
    /// <summary>
    /// Base URL for the PhotoPrism API (e.g., "https://demo.photoprism.app").
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    /// API version to use (default: "v1").
    /// </summary>
    public string ApiVersion { get; set; } = "v1";

    /// <summary>
    /// Username for authentication.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Password for authentication.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Access token for authentication (alternative to username/password).
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Default timeout for HTTP requests.
    /// </summary>
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Maximum number of retry attempts for failed requests.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Base delay for exponential backoff retry strategy.
    /// </summary>
    public TimeSpan RetryBaseDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Maximum delay between retry attempts.
    /// </summary>
    public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// HTTP status codes that should trigger a retry.
    /// </summary>
    public ISet<HttpStatusCode> RetryableStatusCodes { get; set; } = new HashSet<HttpStatusCode>
    {
        HttpStatusCode.RequestTimeout,
        HttpStatusCode.TooManyRequests,
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.GatewayTimeout
    };

    /// <summary>
    /// Maximum number of requests per minute (0 = no limit).
    /// </summary>
    public int RateLimitRequestsPerMinute { get; set; } = 0;

    /// <summary>
    /// Whether to enable response caching.
    /// </summary>
    public bool EnableCaching { get; set; } = true;

    /// <summary>
    /// Default cache TTL for GET requests.
    /// </summary>
    public TimeSpan DefaultCacheTtl { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to enable detailed request/response logging.
    /// </summary>
    public bool EnableVerboseLogging { get; set; } = false;

    /// <summary>
    /// Circuit breaker failure threshold before opening the circuit.
    /// </summary>
    public int CircuitBreakerFailureThreshold { get; set; } = 5;

    /// <summary>
    /// Duration to keep the circuit open before attempting to close it.
    /// </summary>
    public TimeSpan CircuitBreakerOpenDuration { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Gets the full API base URL.
    /// </summary>
    public string ApiBaseUrl => $"{BaseUrl.TrimEnd('/')}/api/{ApiVersion}";

    /// <summary>
    /// Validates the configuration settings.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when configuration is invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(BaseUrl))
            throw new InvalidOperationException("BaseUrl is required");

        if (!Uri.IsWellFormedUriString(BaseUrl, UriKind.Absolute))
            throw new InvalidOperationException("BaseUrl must be a valid absolute URI");

        if (string.IsNullOrWhiteSpace(ApiVersion))
            throw new InvalidOperationException("ApiVersion is required");

        if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(AccessToken))
            throw new InvalidOperationException("Either Username or AccessToken is required");

        if (!string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
            throw new InvalidOperationException("Password is required when Username is provided");

        if (MaxRetryAttempts < 0)
            throw new InvalidOperationException("MaxRetryAttempts cannot be negative");

        if (RateLimitRequestsPerMinute < 0)
            throw new InvalidOperationException("RateLimitRequestsPerMinute cannot be negative");

        if (CircuitBreakerFailureThreshold <= 0)
            throw new InvalidOperationException("CircuitBreakerFailureThreshold must be greater than 0");
    }
}
