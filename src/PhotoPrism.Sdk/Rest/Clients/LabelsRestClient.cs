using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.Core;
using PhotoPrism.Sdk.Rest.Models.Requests;
using PhotoPrism.Sdk.Rest.Models.Responses;

using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk.Rest.Clients;

/// <summary>
/// REST client for PhotoPrism Labels API.
/// </summary>
public class LabelsRestClient(
    HttpClient httpClient,
    RestClientConfiguration configuration,
    ILogger<LabelsRestClient>? logger = null
) : RestClientBase(httpClient, configuration, logger)
{

    /// <summary>
    /// Searches for labels using the specified parameters.
    /// </summary>
    [Operation("labels.search", since: "1.0.0")]
    public async Task<IReadOnlyList<LabelSearchResponse>?> SearchLabelsAsync(
        LabelSearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var queryParams = BuildSearchQueryParameters(request);
        var endpoint = $"labels?{queryParams}";
        var cacheKey = Configuration.EnableCaching ? $"labels:search:{queryParams.GetHashCode()}" : null;

        var response = await GetAsync<LabelSearchResponse[]>(endpoint, cacheKey, cancellationToken: cancellationToken);
        return response ?? Array.Empty<LabelSearchResponse>();
    }

    /// <summary>
    /// Updates label name and properties.
    /// </summary>
    [Operation("labels.update", since: "1.0.0")]
    public async Task<LabelResponse?> UpdateLabelAsync(
        string uid,
        UpdateLabelRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Label UID cannot be null or empty", nameof(uid));

        var endpoint = $"labels/{uid}";

        // Invalidate cache for labels
        if (Configuration.EnableCaching)
        {
            CacheManager.RemoveByPattern("labels:search:*");
        }

        return await PutAsync<UpdateLabelRequest, LabelResponse>(endpoint, request, cancellationToken);
    }

    /// <summary>
    /// Marks a label as favorite.
    /// </summary>
    [Operation("labels.like", since: "1.0.0")]
    public async Task LikeLabelAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Label UID cannot be null or empty", nameof(uid));

        var endpoint = $"labels/{uid}/like";

        // Invalidate cache for labels
        if (Configuration.EnableCaching)
        {
            CacheManager.RemoveByPattern("labels:search:*");
        }

        await PostAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Removes favorite flag from a label.
    /// </summary>
    [Operation("labels.dislike", since: "1.0.0")]
    public async Task DislikeLabelAsync(string uid, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Label UID cannot be null or empty", nameof(uid));

        var endpoint = $"labels/{uid}/like";

        // Invalidate cache for labels
        if (Configuration.EnableCaching)
        {
            CacheManager.RemoveByPattern("labels:search:*");
        }

        await DeleteAsync(endpoint, cancellationToken);
    }

    /// <summary>
    /// Gets a label cover image URL.
    /// </summary>
    [Operation("labels.cover", since: "1.0.0")]
    public string GetLabelCoverUrl(string uid, string size, string token = "public")
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Label UID cannot be null or empty", nameof(uid));

        if (string.IsNullOrWhiteSpace(size))
            throw new ArgumentException("Size cannot be null or empty", nameof(size));

        return $"{Configuration.ApiBaseUrl}/labels/{uid}/t/{token}/{size}";
    }

    /// <summary>
    /// Downloads a label cover image as a stream.
    /// </summary>
    [Operation("labels.cover", since: "1.0.0")]
    public async Task<Stream?> DownloadLabelCoverAsync(
        string uid,
        string size,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(uid))
            throw new ArgumentException("Label UID cannot be null or empty", nameof(uid));

        if (string.IsNullOrWhiteSpace(size))
            throw new ArgumentException("Size cannot be null or empty", nameof(size));

        // Get authentication token
        var token = await AuthManager.GetValidTokenAsync(cancellationToken);
        var endpoint = $"labels/{uid}/t/{token}/{size}";

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
            $"Failed to download label cover: {response.StatusCode}",
            response.StatusCode,
            errorContent);
    }

    private string BuildSearchQueryParameters(LabelSearchRequest request)
    {
        var parameters = new List<string>
        {
            $"count={request.Count}"
        };

        if (request.Offset > 0)
            parameters.Add($"offset={request.Offset}");

        if (request.All)
            parameters.Add($"all={request.All.ToString().ToLower()}");

        if (!string.IsNullOrWhiteSpace(request.Query))
            parameters.Add($"q={Uri.EscapeDataString(request.Query)}");

        return string.Join("&", parameters);
    }
}
