using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.Core;
using PhotoPrism.Sdk.Rest.Models.Requests;
using PhotoPrism.Sdk.Rest.Models.Responses;

using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk.Rest.Clients;

/// <summary>
/// REST client for PhotoPrism Albums API.
/// </summary>
public class AlbumsRestClient(
    HttpClient httpClient,
    RestClientConfiguration configuration,
    ILogger<AlbumsRestClient>? logger = null
) : RestClientBase(httpClient, configuration, logger)
{

    /// <summary>
    /// Searches for albums using the specified parameters.
    /// </summary>
    [Operation("albums.search", since: "1.0.0")]
    public async Task<IReadOnlyList<AlbumSearchResponse>?> SearchAlbumsAsync(
        SearchAlbumsRequest request,
        CancellationToken cancellationToken = default)
    {
        var queryParams = BuildSearchQueryParameters(request);
        var endpoint = $"albums?{queryParams}";
        var cacheKey = Configuration.EnableCaching ? $"albums:search:{queryParams.GetHashCode()}" : null;

        var response = await GetAsync<AlbumSearchResponse[]>(endpoint, cacheKey, cancellationToken: cancellationToken);
        return response ?? Array.Empty<AlbumSearchResponse>();
    }

    /// <summary>
    /// Gets album details by UID.
    /// </summary>
    [Operation("albums.get", since: "1.0.0")]
    public async Task<AlbumResponse?> GetAlbumAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}";
        var cacheKey = Configuration.EnableCaching ? $"album:{uid}" : null;

        return await GetAsync<AlbumResponse>(endpoint, cacheKey, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Creates a new album.
    /// </summary>
    [Operation("albums.create", since: "1.0.0")]
    public async Task<AlbumResponse?> CreateAlbumAsync(
        CreateAlbumRequest request,
        CancellationToken cancellationToken = default)
    {
        var endpoint = "albums";

        // Invalidate album search cache
        if (Configuration.EnableCaching)
        {
            CacheManager.RemoveByPattern("albums:search:*");
        }

        return await PostAsync<CreateAlbumRequest, AlbumResponse>(endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Updates album metadata.
    /// </summary>
    [Operation("albums.update", since: "1.0.0")]
    public async Task<AlbumResponse?> UpdateAlbumAsync(
        string uid,
        UpdateAlbumRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}";

        // Invalidate cache for this album
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"album:{uid}");
            CacheManager.RemoveByPattern("albums:search:*");
        }

        return await PutAsync<UpdateAlbumRequest, AlbumResponse>(endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Deletes an album.
    /// </summary>
    [Operation("albums.delete", since: "1.0.0")]
    public async Task DeleteAlbumAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}";

        // Invalidate cache for this album
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"album:{uid}");
            CacheManager.RemoveByPattern("albums:search:*");
        }

        await DeleteAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Marks an album as favorite.
    /// </summary>
    [Operation("albums.like", since: "1.0.0")]
    public async Task LikeAlbumAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}/like";

        // Invalidate cache for this album
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"album:{uid}");
            CacheManager.RemoveByPattern("albums:search:*");
        }

        await PostAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Removes favorite flag from an album.
    /// </summary>
    [Operation("albums.dislike", since: "1.0.0")]
    public async Task DislikeAlbumAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}/like";

        // Invalidate cache for this album
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"album:{uid}");
            CacheManager.RemoveByPattern("albums:search:*");
        }

        await DeleteAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Adds photos to an album.
    /// </summary>
    [Operation("albums.addPhotos", since: "1.0.0")]
    public async Task AddPhotosToAlbumAsync(
        string uid,
        AddPhotosToAlbumRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}/photos";

        // Invalidate cache for this album
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"album:{uid}");
            CacheManager.RemoveByPattern("albums:search:*");
        }

        await PostAsync(endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Removes photos from an album.
    /// </summary>
    [Operation("albums.removePhotos", since: "1.0.0")]
    public async Task RemovePhotosFromAlbumAsync(
        string uid,
        RemovePhotosFromAlbumRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}/photos";

        // Invalidate cache for this album
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"album:{uid}");
            CacheManager.RemoveByPattern("albums:search:*");
        }

        await DeleteAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Downloads an album as a ZIP file.
    /// </summary>
    [Operation("albums.download", since: "1.0.0")]
    public async Task<Stream?> DownloadAlbumAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        var endpoint = $"albums/{uid}/dl";

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
            $"Failed to download album: {response.StatusCode}",
            response.StatusCode,
            errorContent);
    }

    /// <summary>
    /// Gets a download URL for an album.
    /// </summary>
    [Operation("albums.download", since: "1.0.0")]
    public string GetAlbumDownloadUrl(string uid)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Album UID cannot be null or empty", nameof(uid));

        return $"{Configuration.ApiBaseUrl}/albums/{uid}/dl";
    }

    private string BuildSearchQueryParameters(SearchAlbumsRequest request)
    {
        var parameters = new List<string>
        {
            $"count={request.Count}"
        };

        if (request.Offset > 0)
            parameters.Add($"offset={request.Offset}");

        if (!string.IsNullOrWhiteSpace(request.Order))
            parameters.Add($"order={Uri.EscapeDataString(request.Order)}");

        if (!string.IsNullOrWhiteSpace(request.Query))
            parameters.Add($"q={Uri.EscapeDataString(request.Query)}");

        return string.Join("&", parameters);
    }
}
