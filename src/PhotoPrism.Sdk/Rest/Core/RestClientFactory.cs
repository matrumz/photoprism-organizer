using System.Net.Http;

using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.Clients;

namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// Factory for creating and configuring PhotoPrism REST API clients.
/// </summary>
public class RestClientFactory(
    RestClientConfiguration configuration,
    Func<HttpClient>? httpClientFactory = null,
    ILoggerFactory? loggerFactory = null
)
{
    private readonly RestClientConfiguration _configuration = ValidateAndReturn(configuration);
    private readonly Func<HttpClient>? _httpClientFactory = httpClientFactory;
    private readonly ILoggerFactory? _loggerFactory = loggerFactory;

    private static RestClientConfiguration ValidateAndReturn(RestClientConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        configuration.Validate();
        return configuration;
    }

    /// <summary>
    /// Creates a photos REST client.
    /// </summary>
    /// <returns>Configured photos client.</returns>
    public PhotosRestClient CreatePhotosClient()
    {
        var httpClient = CreateHttpClient();
        var logger = _loggerFactory?.CreateLogger<PhotosRestClient>();
        return new PhotosRestClient(httpClient, _configuration, logger);
    }

    /// <summary>
    /// Creates an albums REST client.
    /// </summary>
    /// <returns>Configured albums client.</returns>
    public AlbumsRestClient CreateAlbumsClient()
    {
        var httpClient = CreateHttpClient();
        var logger = _loggerFactory?.CreateLogger<AlbumsRestClient>();
        return new AlbumsRestClient(httpClient, _configuration, logger);
    }

    /// <summary>
    /// Creates a labels REST client.
    /// </summary>
    /// <returns>Configured labels client.</returns>
    public LabelsRestClient CreateLabelsClient()
    {
        var httpClient = CreateHttpClient();
        var logger = _loggerFactory?.CreateLogger<LabelsRestClient>();
        return new LabelsRestClient(httpClient, _configuration, logger);
    }

    /// <summary>
    /// Creates a files REST client.
    /// </summary>
    /// <returns>Configured files client.</returns>
    public FilesRestClient CreateFilesClient()
    {
        var httpClient = CreateHttpClient();
        var logger = _loggerFactory?.CreateLogger<FilesRestClient>();
        return new FilesRestClient(httpClient, _configuration, logger);
    }

    /// <summary>
    /// Creates an HTTP client with appropriate configuration.
    /// </summary>
    /// <returns>Configured HTTP client.</returns>
    private HttpClient CreateHttpClient()
    {
        var httpClient = _httpClientFactory?.Invoke() ?? new HttpClient();

        // Configure base address
        if (httpClient.BaseAddress == null)
        {
            httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);
        }

        // Configure timeout
        httpClient.Timeout = _configuration.DefaultTimeout;

        // Configure default headers
        httpClient.DefaultRequestHeaders.Add("User-Agent", "PhotoPrism.Sdk/1.0.0");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        return httpClient;
    }

    /// <summary>
    /// Gets the current configuration.
    /// </summary>
    public RestClientConfiguration Configuration => _configuration;

    /// <summary>
    /// Creates a new factory with updated configuration.
    /// </summary>
    /// <param name="configure">Configuration update action.</param>
    /// <returns>New factory instance with updated configuration.</returns>
    public RestClientFactory WithConfiguration(Action<RestClientConfiguration> configure)
    {
        var newConfig = new RestClientConfiguration
        {
            BaseUrl = _configuration.BaseUrl,
            ApiVersion = _configuration.ApiVersion,
            Username = _configuration.Username,
            Password = _configuration.Password,
            AccessToken = _configuration.AccessToken,
            DefaultTimeout = _configuration.DefaultTimeout,
            MaxRetryAttempts = _configuration.MaxRetryAttempts,
            RetryBaseDelay = _configuration.RetryBaseDelay,
            MaxRetryDelay = _configuration.MaxRetryDelay,
            RetryableStatusCodes = new HashSet<System.Net.HttpStatusCode>(_configuration.RetryableStatusCodes),
            RateLimitRequestsPerMinute = _configuration.RateLimitRequestsPerMinute,
            EnableCaching = _configuration.EnableCaching,
            DefaultCacheTtl = _configuration.DefaultCacheTtl,
            EnableVerboseLogging = _configuration.EnableVerboseLogging,
            CircuitBreakerFailureThreshold = _configuration.CircuitBreakerFailureThreshold,
            CircuitBreakerOpenDuration = _configuration.CircuitBreakerOpenDuration
        };

        configure(newConfig);
        return new RestClientFactory(newConfig, _httpClientFactory, _loggerFactory);
    }

    /// <summary>
    /// Creates a new factory with different authentication.
    /// </summary>
    /// <param name="username">Username for authentication.</param>
    /// <param name="password">Password for authentication.</param>
    /// <returns>New factory instance with updated authentication.</returns>
    public RestClientFactory WithCredentials(string username, string password)
    {
        return WithConfiguration(config =>
        {
            config.Username = username;
            config.Password = password;
            config.AccessToken = null; // Clear access token when using credentials
        });
    }

    /// <summary>
    /// Creates a new factory with access token authentication.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>New factory instance with updated authentication.</returns>
    public RestClientFactory WithAccessToken(string accessToken)
    {
        return WithConfiguration(config =>
        {
            config.AccessToken = accessToken;
            config.Username = null; // Clear username/password when using token
            config.Password = null;
        });
    }
}
