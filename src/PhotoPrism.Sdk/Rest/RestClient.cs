using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using PhotoPrismOrganizer.Common.Apis.Exceptions;
using PhotoPrismOrganizer.Common.Extensions;

namespace PhotoPrism.Sdk.Rest;

internal abstract class RestClient(
    IHttpClientFactory httpClientFactory,
    ILogger<RestClient> logger
)
{

    private static readonly JsonSerializerOptions DefaultResponseDeserializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private HttpClient PhotoPrismHttpClient => httpClientFactory.CreateClient("PhotoPrism");

    /// <summary>
    /// Builds a URI for the specified path and query parameters.
    /// </summary>
    /// <param name="path">The path of the URI.</param>
    /// <param name="query">An object representing the query parameters.</param>
    /// <returns>The constructed URI.</returns>
    /// <remarks>
    /// The <paramref name="query"/> object can be an anonymous object where each property represents a query parameter.
    /// <br/>List properties will result in repeated query parameters.
    /// </remarks>
    private static string BuildUri(string? path = null, object? query = null)
    {
        var queryString = QueryString.Create(query.ToKeyValuePairs(includeNullValues: false) ?? []).ToUriComponent();
        return string.IsNullOrEmpty(queryString) ? (path ?? "/") : $"{path ?? "/"}{queryString}";
    }

    protected Task<TInterface> GetAsync<TInterface, TConcrete>(
        string path,
        object? queryParams = null,
        string? operationContext = null,
        CancellationToken cancellationToken = default
    ) where TConcrete : class, TInterface =>
        ExecuteRestRequestAsync<TInterface, TConcrete>(
            () => PhotoPrismHttpClient.GetAsync(BuildUri(path, queryParams), cancellationToken),
            operationContext ?? $"GET {path}",
            cancellationToken: cancellationToken
        );

    private static async Task<TInterface> ExecuteRestRequestAsync<TInterface, TConcrete>(
        Func<Task<HttpResponseMessage>> httpRequestFunction,
        string operationContext,
        JsonSerializerOptions? jsonOptions = null,
        CancellationToken cancellationToken = default
    ) where TConcrete : class, TInterface
    {
        try
        {
            var response = await httpRequestFunction();

            // Handle HTTP status codes and throw appropriate exceptions
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                throw ApiExceptionFactory.CreateHttpException(response.StatusCode, content, operationContext);
            }

            // Deserialize successful response
            return await DeserializeResponseAsync<TInterface, TConcrete>(response, jsonOptions, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw ApiExceptionFactory.CreateNetworkException(operationContext, ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw ApiExceptionFactory.CreateTimeoutException(operationContext, ex);
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            throw ApiExceptionFactory.CreateCancellationException(operationContext, ex);
        }
        catch (JsonException ex)
        {
            throw ApiExceptionFactory.CreateJsonException(operationContext, ex);
        }
    }

    private static async Task<TInterface> DeserializeResponseAsync<TInterface, TConcrete>(
        HttpResponseMessage response,
        JsonSerializerOptions? jsonOptions = null,
        CancellationToken cancellationToken = default
    ) where TConcrete : class, TInterface
    {
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<TConcrete>(responseContent, jsonOptions ?? DefaultResponseDeserializationOptions);
        return result ?? throw new InvalidOperationException($"Failed to deserialize response to {typeof(TConcrete).Name}");
    }

}
