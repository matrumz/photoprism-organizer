using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.Core;
using PhotoPrism.Sdk.Rest.Models.Requests;
using PhotoPrism.Sdk.Rest.Models.Responses;

using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk.Rest.Clients;

/// <summary>
/// REST client for PhotoPrism Photos API.
/// </summary>
public class PhotosRestClient(
    HttpClient httpClient,
    RestClientConfiguration configuration,
    ILogger<PhotosRestClient>? logger = null
) : RestClientBase(httpClient, configuration, logger)
{
    /// <summary>
    /// Searches for photos using the specified parameters.
    /// </summary>
    [Operation("photos.search", since: "1.0.0")]
    public async Task<IReadOnlyList<PhotoSearchResponse>?> SearchPhotosAsync(
        SearchPhotosRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var queryParams = BuildSearchQueryParameters(request);
        var endpoint = $"photos?{queryParams}";
        var cacheKey = Configuration.EnableCaching ? $"photos:search:{queryParams.GetHashCode()}" : null;

        var response = await GetAsync<PhotoSearchResponse[]>(endpoint, cacheKey, cancellationToken: cancellationToken);
        return response ?? Array.Empty<PhotoSearchResponse>();
    }

    /// <summary>
    /// Gets photo details by UID.
    /// </summary>
    [Operation("photos.get", since: "1.0.0")]
    public async Task<PhotoResponse?> GetPhotoAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}";
        var cacheKey = Configuration.EnableCaching ? $"photo:{uid}" : null;

        return await GetAsync<PhotoResponse>(endpoint, cacheKey, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Updates photo metadata.
    /// </summary>
    [Operation("photos.update", since: "1.0.0")]
    public async Task<PhotoResponse?> UpdatePhotoAsync(
        string uid,
        UpdatePhotoRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}";

        // Invalidate cache for this photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{uid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        return await PutAsync<UpdatePhotoRequest, PhotoResponse>(endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Marks a photo as favorite.
    /// </summary>
    [Operation("photos.like", since: "1.0.0")]
    public async Task LikePhotoAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}/like";

        // Invalidate cache for this photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{uid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        await PostAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Removes favorite flag from a photo.
    /// </summary>
    [Operation("photos.dislike", since: "1.0.0")]
    public async Task DislikePhotoAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}/like";

        // Invalidate cache for this photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{uid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        await DeleteAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Approves a photo that is currently under review.
    /// </summary>
    [Operation("photos.approve", since: "1.0.0")]
    public async Task ApprovePhotoAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}/approve";

        // Invalidate cache for this photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{uid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        await PostAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Gets a download URL for a photo.
    /// </summary>
    [Operation("photos.download", since: "1.0.0")]
    public string GetPhotoDownloadUrl(string uid)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        return $"{Configuration.ApiBaseUrl}/photos/{uid}/dl";
    }

    /// <summary>
    /// Downloads photo data as a stream.
    /// </summary>
    [Operation("photos.download", since: "1.0.0")]
    public async Task<Stream?> DownloadPhotoAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}/dl";

        // Get authentication token
        var token = await AuthManager.GetValidTokenAsync(cancellationToken);

        // Create request with authentication
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new Exceptions.ApiException(
            $"Failed to download photo: {response.StatusCode}",
            response.StatusCode,
            errorContent);
    }

    /// <summary>
    /// Gets photo details in YAML format.
    /// </summary>
    [Operation("photos.yaml", since: "1.0.0")]
    public async Task<string?> GetPhotoYamlAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(uid));

        var endpoint = $"photos/{uid}/yaml";

        // Get authentication token
        var token = await AuthManager.GetValidTokenAsync(cancellationToken);

        // Create request with authentication
        using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await HttpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new Exceptions.ApiException(
            $"Failed to get photo YAML: {response.StatusCode}",
            response.StatusCode,
            errorContent);
    }

    private string BuildSearchQueryParameters(SearchPhotosRequest request)
    {
        var parameters = new List<string>
        {
            $"count={request.Count}"
        };

        if (request.Offset > 0)
            parameters.Add($"offset={request.Offset}");

        if (!string.IsNullOrWhiteSpace(request.Order))
            parameters.Add($"order={Uri.EscapeDataString(request.Order)}");

        if (request.Merged.HasValue)
            parameters.Add($"merged={request.Merged.Value.ToString().ToLower()}");

        if (request.Public.HasValue)
            parameters.Add($"public={request.Public.Value.ToString().ToLower()}");

        if (request.Quality.HasValue)
            parameters.Add($"quality={request.Quality.Value}");

        if (!string.IsNullOrWhiteSpace(request.Query))
            parameters.Add($"q={Uri.EscapeDataString(request.Query)}");

        if (!string.IsNullOrWhiteSpace(request.AlbumUid))
            parameters.Add($"s={Uri.EscapeDataString(request.AlbumUid)}");

        if (!string.IsNullOrWhiteSpace(request.Path))
            parameters.Add($"path={Uri.EscapeDataString(request.Path)}");

        if (request.Video.HasValue)
            parameters.Add($"video={request.Video.Value.ToString().ToLower()}");

        return string.Join("&", parameters);
    }
}
