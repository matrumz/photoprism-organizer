using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.Exceptions;

namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// Abstract base class for PhotoPrism REST API clients.
/// </summary>
public abstract class RestClientBase(
    HttpClient httpClient,
    RestClientConfiguration configuration,
    ILogger? logger = null
) : IDisposable
{
    protected readonly HttpClient HttpClient = httpClient;
    protected readonly RestClientConfiguration Configuration = configuration;
    protected readonly ILogger? Logger = logger;
    protected readonly AuthenticationManager AuthManager = new(httpClient, configuration);
    protected readonly RetryPolicy RetryPolicy = new(configuration);
    protected readonly RateLimiter RateLimiter = new(configuration.RateLimitRequestsPerMinute);
    protected readonly CacheManager CacheManager = new(configuration.DefaultCacheTtl);
    protected readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private readonly SemaphoreSlim _circuitBreakerSemaphore = new(1, 1);
    private int _failureCount = 0;
    private DateTimeOffset _circuitOpenedAt = DateTimeOffset.MinValue;
    private bool _circuitOpen = false;

    /// <summary>
    /// Executes a GET request with retry logic and caching.
    /// </summary>
    /// <typeparam name="T">Response type.</typeparam>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="cacheKey">Cache key (if null, no caching is used).</param>
    /// <param name="cacheTtl">Cache TTL (if null, uses default).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<T?> GetAsync<T>(
        string endpoint,
        string? cacheKey = null,
        TimeSpan? cacheTtl = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        // Check cache first
        if (cacheKey != null && Configuration.EnableCaching)
        {
            var cached = CacheManager.Get<T>(cacheKey);
            if (cached != null)
            {
                Logger?.LogDebug("Cache hit for key: {CacheKey}", cacheKey);
                return cached;
            }
        }

        var result = await ExecuteRequestAsync<T>(HttpMethod.Get, endpoint, cancellationToken: cancellationToken);

        // Cache the result
        if (result != null && cacheKey != null && Configuration.EnableCaching)
        {
            CacheManager.Set(cacheKey, result, cacheTtl ?? Configuration.DefaultCacheTtl);
            Logger?.LogDebug("Cached response for key: {CacheKey}", cacheKey);
        }

        return result;
    }

    /// <summary>
    /// Executes a POST request with retry logic.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="request">Request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default
    ) where TResponse : class
    {
        return await ExecuteRequestAsync<TResponse>(HttpMethod.Post, endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Executes a POST request without expecting a response body.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="request">Request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected async Task PostAsync<TRequest>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await ExecuteRequestAsync<object>(HttpMethod.Post, endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Executes a POST request without a request body.
    /// </summary>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected async Task PostAsync(
        string endpoint,
        CancellationToken cancellationToken = default
    )
    {
        await ExecuteRequestAsync<object>(HttpMethod.Post, endpoint, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Executes a PUT request with retry logic.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="request">Request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<TResponse?> PutAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default
    ) where TResponse : class
    {
        return await ExecuteRequestAsync<TResponse>(HttpMethod.Put, endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Executes a PUT request without expecting a response body.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="request">Request body.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected async Task PutAsync<TRequest>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default
    )
    {
        await ExecuteRequestAsync<object>(HttpMethod.Put, endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Executes a DELETE request with retry logic.
    /// </summary>
    /// <typeparam name="T">Response type.</typeparam>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Deserialized response.</returns>
    protected async Task<T?> DeleteAsync<T>(
        string endpoint,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        return await ExecuteRequestAsync<T>(HttpMethod.Delete, endpoint, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Executes a DELETE request without expecting a response body.
    /// </summary>
    /// <param name="endpoint">API endpoint (relative to base URL).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected async Task DeleteAsync(
        string endpoint,
        CancellationToken cancellationToken = default
    )
    {
        await ExecuteRequestAsync<object>(HttpMethod.Delete, endpoint, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Executes an HTTP request with circuit breaker and retry logic.
    /// </summary>
    private async Task<T?> ExecuteRequestAsync<T>(
        HttpMethod method,
        string endpoint,
        object? requestBody = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        // Apply rate limiting
        await RateLimiter.AcquireAsync(cancellationToken);

        return await RetryPolicy.ExecuteAsync(async ct =>
        {
            var result = await SendRequestAsync<T>(method, endpoint, requestBody, ct);
            await RecordSuccess();
            return result;
        }, cancellationToken);
    }

    private async Task<T?> SendRequestAsync<T>(
        HttpMethod method,
        string endpoint,
        object? requestBody = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        // Get authentication token
        var token = await AuthManager.GetValidTokenAsync(cancellationToken);

        // Create request
        using var request = new HttpRequestMessage(method, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (requestBody != null)
        {
            var json = JsonSerializer.Serialize(requestBody, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (Configuration.EnableVerboseLogging)
        {
            Logger?.LogDebug("Sending {Method} request to {Endpoint}", method, endpoint);
            if (requestBody != null)
            {
                Logger?.LogDebug("Request body: {RequestBody}", JsonSerializer.Serialize(requestBody, JsonOptions));
            }
        }

        var response = await HttpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (Configuration.EnableVerboseLogging)
        {
            Logger?.LogDebug("Received {StatusCode} response: {ResponseContent}", response.StatusCode, responseContent);
        }

        if (response.IsSuccessStatusCode)
        {
            if (string.IsNullOrWhiteSpace(responseContent))
                return null;

            try
            {
                return JsonSerializer.Deserialize<T>(responseContent, JsonOptions);
            }
            catch (JsonException ex)
            {
                Logger?.LogError(ex, "Failed to deserialize response: {ResponseContent}", responseContent);
                throw new ApiException(
                    "Failed to deserialize API response",
                    response.StatusCode,
                    responseContent,
                    isRetryable: false);
            }
        }

        throw CreateApiException(response, responseContent);
    }

    private ApiException CreateApiException(HttpResponseMessage response, string responseContent)
    {
        var statusCode = response.StatusCode;
        var isRetryable = Configuration.RetryableStatusCodes.Contains(statusCode);

        // Try to parse PhotoPrism error response
        try
        {
            var errorResponse = JsonSerializer.Deserialize<PhotoPrismErrorResponse>(responseContent, JsonOptions);
            if (errorResponse != null)
            {
                return statusCode switch
                {
                    HttpStatusCode.BadRequest => new BadRequestException(errorResponse.Message ?? "Bad request", errorResponse.Details),
                    HttpStatusCode.Unauthorized => new AuthenticationException(errorResponse.Message ?? "Unauthorized"),
                    HttpStatusCode.Forbidden => new ForbiddenException(errorResponse.Message ?? "Forbidden"),
                    HttpStatusCode.NotFound => new NotFoundException(errorResponse.Message ?? "Not found"),
                    HttpStatusCode.TooManyRequests => new RateLimitException(errorResponse.Message ?? "Rate limit exceeded", GetRetryAfter(response)),
                    _ when statusCode >= HttpStatusCode.InternalServerError => new ServerException(errorResponse.Message ?? "Server error", statusCode, responseContent),
                    _ => new ApiException(errorResponse.Message ?? $"API error: {statusCode}", statusCode, responseContent, errorResponse.Code, errorResponse.Details, isRetryable)
                };
            }
        }
        catch (JsonException)
        {
            // Fall through to generic error handling
        }

        // Generic error response
        return statusCode switch
        {
            HttpStatusCode.BadRequest => new BadRequestException("Bad request", responseContent),
            HttpStatusCode.Unauthorized => new AuthenticationException("Unauthorized"),
            HttpStatusCode.Forbidden => new ForbiddenException("Forbidden"),
            HttpStatusCode.NotFound => new NotFoundException("Not found"),
            HttpStatusCode.TooManyRequests => new RateLimitException("Rate limit exceeded", GetRetryAfter(response)),
            _ when statusCode >= HttpStatusCode.InternalServerError => new ServerException("Server error", statusCode, responseContent),
            _ => new ApiException($"HTTP {(int)statusCode}: {statusCode}", statusCode, responseContent, isRetryable: isRetryable)
        };
    }

    private DateTimeOffset? GetRetryAfter(HttpResponseMessage response)
    {
        if (response.Headers.RetryAfter?.Delta.HasValue == true)
        {
            return DateTimeOffset.UtcNow.Add(response.Headers.RetryAfter.Delta.Value);
        }

        if (response.Headers.RetryAfter?.Date.HasValue == true)
        {
            return response.Headers.RetryAfter.Date.Value;
        }

        return null;
    }

    private async Task RecordSuccess()
    {
        await _circuitBreakerSemaphore.WaitAsync();
        try
        {
            _failureCount = 0;
            if (_circuitOpen)
            {
                _circuitOpen = false;
            }
        }
        finally
        {
            _circuitBreakerSemaphore.Release();
        }
    }

    private async Task RecordFailure()
    {
        await _circuitBreakerSemaphore.WaitAsync();
        try
        {
            _failureCount++;
            if (_failureCount >= Configuration.CircuitBreakerFailureThreshold && !_circuitOpen)
            {
                _circuitOpen = true;
                _circuitOpenedAt = DateTimeOffset.UtcNow;
                Logger?.LogWarning("Circuit breaker opened after {FailureCount} failures", _failureCount);
            }
        }
        catch (Exception)
        {
            // Error during circuit breaker state management - continue with original exception
        }
        finally
        {
            _circuitBreakerSemaphore.Release();
        }
    }

    /// <summary>
    /// Disposes the client and its resources.
    /// </summary>
    public virtual void Dispose()
    {
        _circuitBreakerSemaphore?.Dispose();
        CacheManager?.Dispose();
        // HttpClient is not disposed here as it should be managed by HttpClientFactory
        // RateLimiter and AuthManager don't implement IDisposable
    }

    private class PhotoPrismErrorResponse
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
        public string? Error { get; set; }
    }
}
