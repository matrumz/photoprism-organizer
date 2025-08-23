using System.Net.Http.Json;
using System.Text.Json;

using PhotoPrism.Sdk.Rest.Exceptions;

namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// Manages PhotoPrism authentication and session tokens.
/// </summary>
public class AuthenticationManager
{
    private readonly HttpClient _httpClient;
    private readonly RestClientConfiguration _configuration;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _authSemaphore = new(1, 1);

    private string? _accessToken;
    private DateTimeOffset _tokenExpiresAt = DateTimeOffset.MinValue;

    public AuthenticationManager(HttpClient httpClient, RestClientConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        // If access token is provided in configuration, use it
        if (!string.IsNullOrWhiteSpace(_configuration.AccessToken))
        {
            _accessToken = _configuration.AccessToken;
            _tokenExpiresAt = DateTimeOffset.MaxValue; // Assume it doesn't expire unless we get a 401
        }
    }

    /// <summary>
    /// Gets a valid access token, refreshing if necessary.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Valid access token.</returns>
    public async Task<string> GetValidTokenAsync(CancellationToken cancellationToken = default)
    {
        // If we have a token that's not expired, return it
        if (!string.IsNullOrWhiteSpace(_accessToken) && _tokenExpiresAt > DateTimeOffset.UtcNow.AddMinutes(5))
        {
            return _accessToken;
        }

        await _authSemaphore.WaitAsync(cancellationToken);
        try
        {
            // Double-check after acquiring the lock
            if (!string.IsNullOrWhiteSpace(_accessToken) && _tokenExpiresAt > DateTimeOffset.UtcNow.AddMinutes(5))
            {
                return _accessToken;
            }

            // Authenticate to get a new token
            await AuthenticateAsync(cancellationToken);
            return _accessToken ?? throw new AuthenticationException("Failed to obtain access token");
        }
        finally
        {
            _authSemaphore.Release();
        }
    }

    /// <summary>
    /// Invalidates the current token, forcing re-authentication on next request.
    /// </summary>
    public void InvalidateToken()
    {
        _accessToken = null;
        _tokenExpiresAt = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Checks if we currently have a valid token.
    /// </summary>
    /// <returns>True if token is valid, false otherwise.</returns>
    public bool HasValidToken()
    {
        return !string.IsNullOrWhiteSpace(_accessToken) && _tokenExpiresAt > DateTimeOffset.UtcNow.AddMinutes(5);
    }

    private async Task AuthenticateAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_configuration.Username) || string.IsNullOrWhiteSpace(_configuration.Password))
        {
            throw new AuthenticationException("Username and password are required for authentication");
        }

        var loginRequest = new
        {
            username = _configuration.Username,
            password = _configuration.Password
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{_configuration.ApiBaseUrl}/session",
                loginRequest,
                _jsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new AuthenticationException($"Authentication failed: {response.StatusCode} - {errorContent}");
            }

            var sessionResponse = await response.Content.ReadFromJsonAsync<SessionResponse>(_jsonOptions, cancellationToken);

            if (sessionResponse?.AccessToken == null)
            {
                throw new AuthenticationException("Authentication response did not contain an access token");
            }

            _accessToken = sessionResponse.AccessToken;

            // PhotoPrism sessions typically last 24 hours, but we'll be conservative
            _tokenExpiresAt = DateTimeOffset.UtcNow.AddHours(23);
        }
        catch (HttpRequestException ex)
        {
            throw new AuthenticationException("Failed to connect to PhotoPrism server", ex);
        }
        catch (JsonException ex)
        {
            throw new AuthenticationException("Failed to parse authentication response", ex);
        }
    }

    private class SessionResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int? ExpiresIn { get; set; }
    }
}
