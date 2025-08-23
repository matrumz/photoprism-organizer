using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.Core;
using PhotoPrism.Sdk.Rest.Models.Requests;
using PhotoPrism.Sdk.Rest.Models.Responses;

using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk.Rest.Clients;

/// <summary>
/// REST client for PhotoPrism Files API.
/// </summary>
public class FilesRestClient(
    HttpClient httpClient,
    RestClientConfiguration configuration,
    ILogger<FilesRestClient>? logger = null
) : RestClientBase(httpClient, configuration, logger)
{

    /// <summary>
    /// Gets file details by hash.
    /// </summary>
    [Operation("files.get", since: "1.0.0")]
    public async Task<FileResponse?> GetFileAsync(string hash, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("File hash cannot be null or empty", nameof(hash));

        var endpoint = $"files/{hash}";
        var cacheKey = Configuration.EnableCaching ? $"file:{hash}" : null;

        return await GetAsync<FileResponse>(endpoint, cacheKey, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Deletes a file from storage.
    /// </summary>
    [Operation("files.delete", since: "1.0.0")]
    public async Task<PhotoResponse?> DeleteFileAsync(
        string photoUid,
        string fileUid,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(photoUid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(photoUid));

        if (string.IsNullOrWhiteSpace(fileUid))
            throw new ArgumentException("File UID cannot be null or empty", nameof(fileUid));

        var endpoint = $"photos/{photoUid}/files/{fileUid}";

        // Invalidate cache for the photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{photoUid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        return await DeleteAsync<PhotoResponse>(endpoint, cancellationToken);
    }

    /// <summary>
    /// Changes the orientation of a file.
    /// </summary>
    [Operation("files.changeOrientation", since: "1.0.0")]
    public async Task<PhotoResponse?> ChangeFileOrientationAsync(
        string photoUid,
        string fileUid,
        ChangeFileOrientationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(photoUid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(photoUid));

        if (string.IsNullOrWhiteSpace(fileUid))
            throw new ArgumentException("File UID cannot be null or empty", nameof(fileUid));

        var endpoint = $"photos/{photoUid}/files/{fileUid}/orientation";

        // Invalidate cache for the photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{photoUid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        return await PutAsync<ChangeFileOrientationRequest, PhotoResponse>(endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Sets the primary file for a photo.
    /// </summary>
    [Operation("files.setPrimary", since: "1.0.0")]
    public async Task<PhotoResponse?> SetPrimaryFileAsync(
        string photoUid,
        string fileUid,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(photoUid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(photoUid));

        if (string.IsNullOrWhiteSpace(fileUid))
            throw new ArgumentException("File UID cannot be null or empty", nameof(fileUid));

        var endpoint = $"photos/{photoUid}/files/{fileUid}/primary";

        // Invalidate cache for the photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{photoUid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        return await PostAsync<object, PhotoResponse>(endpoint, new { }, cancellationToken);
    }

    /// <summary>
    /// Removes a file from an existing photo stack (unstacks).
    /// </summary>
    [Operation("files.unstack", since: "1.0.0")]
    public async Task<PhotoResponse?> UnstackFileAsync(
        string photoUid,
        string fileUid,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(photoUid))
            throw new ArgumentException("Photo UID cannot be null or empty", nameof(photoUid));

        if (string.IsNullOrWhiteSpace(fileUid))
            throw new ArgumentException("File UID cannot be null or empty", nameof(fileUid));

        var endpoint = $"photos/{photoUid}/files/{fileUid}/unstack";

        // Invalidate cache for the photo
        if (Configuration.EnableCaching)
        {
            CacheManager.Remove($"photo:{photoUid}");
            CacheManager.RemoveByPattern("photos:search:*");
        }

        return await PostAsync<object, PhotoResponse>(endpoint, new { }, cancellationToken);
    }

    /// <summary>
    /// Downloads raw file data as a stream.
    /// </summary>
    [Operation("files.download", since: "1.0.0")]
    public async Task<Stream?> DownloadFileAsync(string hash, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("File hash cannot be null or empty", nameof(hash));

        var endpoint = $"dl/{hash}";

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
            $"Failed to download file: {response.StatusCode}",
            response.StatusCode,
            errorContent);
    }

    /// <summary>
    /// Gets a download URL for a file.
    /// </summary>
    [Operation("files.download", since: "1.0.0")]
    public string GetFileDownloadUrl(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("File hash cannot be null or empty", nameof(hash));

        return $"{Configuration.ApiBaseUrl}/dl/{hash}";
    }

    /// <summary>
    /// Gets a thumbnail URL for a file.
    /// </summary>
    [Operation("files.thumbnail", since: "1.0.0")]
    public string GetThumbnailUrl(string thumb, string size, string token = "public")
    {
        if (string.IsNullOrWhiteSpace(thumb))
            throw new ArgumentException("Thumb cannot be null or empty", nameof(thumb));

        if (string.IsNullOrWhiteSpace(size))
            throw new ArgumentException("Size cannot be null or empty", nameof(size));

        return $"{Configuration.ApiBaseUrl}/t/{thumb}/{token}/{size}";
    }

    /// <summary>
    /// Downloads a thumbnail as a stream.
    /// </summary>
    [Operation("files.thumbnail", since: "1.0.0")]
    public async Task<Stream?> DownloadThumbnailAsync(
        string thumb,
        string size,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(thumb))
            throw new ArgumentException("Thumb cannot be null or empty", nameof(thumb));

        if (string.IsNullOrWhiteSpace(size))
            throw new ArgumentException("Size cannot be null or empty", nameof(size));

        // Get authentication token
        var token = await AuthManager.GetValidTokenAsync(cancellationToken);
        var endpoint = $"t/{thumb}/{token}/{size}";

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
            $"Failed to download thumbnail: {response.StatusCode}",
            response.StatusCode,
            errorContent);
    }

    private new async Task<T?> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken) where T : class
    {
        // Apply rate limiting
        await RateLimiter.AcquireAsync(cancellationToken);

        return await RetryPolicy.ExecuteAsync(async ct =>
        {
            // Get authentication token
            var token = await AuthManager.GetValidTokenAsync(ct);

            // Create request with authentication
            using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await HttpClient.SendAsync(request, ct);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(ct);
                if (string.IsNullOrWhiteSpace(responseContent))
                    return null;

                return System.Text.Json.JsonSerializer.Deserialize<T>(responseContent, JsonOptions);
            }

            var errorContent = await response.Content.ReadAsStringAsync(ct);
            throw new Exceptions.ApiException(
                $"Delete request failed: {response.StatusCode}",
                response.StatusCode,
                errorContent);
        }, cancellationToken);
    }
}
