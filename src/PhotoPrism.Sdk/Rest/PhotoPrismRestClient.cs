using PhotoPrism.Sdk.Rest.Core;
using PhotoPrism.Sdk.Rest.Clients;
using Microsoft.Extensions.Logging;

namespace PhotoPrism.Sdk.Rest;

/// <summary>
/// Main entry point for PhotoPrism REST API clients.
/// Provides a unified interface for accessing all PhotoPrism API endpoints.
/// </summary>
public class PhotoPrismRestClient(
    RestClientConfiguration configuration,
    HttpClient? httpClient = null,
    ILoggerFactory? loggerFactory = null
) : IDisposable
{
    private readonly RestClientFactory _factory = CreateFactory(configuration, httpClient, loggerFactory);
    private readonly bool _disposeFactory = true;

    private PhotosRestClient? _photos;
    private AlbumsRestClient? _albums;
    private LabelsRestClient? _labels;
    private FilesRestClient? _files;

    private static RestClientFactory CreateFactory(
        RestClientConfiguration configuration,
        HttpClient? httpClient,
        ILoggerFactory? loggerFactory
    )
    {
        var httpClientFactory = httpClient != null ? () => httpClient : (Func<HttpClient>?)null;
        return new RestClientFactory(configuration, httpClientFactory, loggerFactory);
    }

    /// <summary>
    /// Initializes a new instance of the PhotoPrismRestClient with a factory.
    /// </summary>
    /// <param name="factory">Pre-configured REST client factory.</param>
    public PhotoPrismRestClient(RestClientFactory factory) : this(factory.Configuration)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        // Override the _disposeFactory field since we don't own this factory
        _disposeFactory = false;
    }

    /// <summary>
    /// Gets the photos client for photo-related operations.
    /// </summary>
    public PhotosRestClient Photos
    {
        get
        {
            return _photos ??= _factory.CreatePhotosClient();
        }
    }

    /// <summary>
    /// Gets the albums client for album-related operations.
    /// </summary>
    public AlbumsRestClient Albums
    {
        get
        {
            return _albums ??= _factory.CreateAlbumsClient();
        }
    }

    /// <summary>
    /// Gets the labels client for label-related operations.
    /// </summary>
    public LabelsRestClient Labels
    {
        get
        {
            return _labels ??= _factory.CreateLabelsClient();
        }
    }

    /// <summary>
    /// Gets the files client for file-related operations.
    /// </summary>
    public FilesRestClient Files
    {
        get
        {
            return _files ??= _factory.CreateFilesClient();
        }
    }

    /// <summary>
    /// Gets the current configuration.
    /// </summary>
    public RestClientConfiguration Configuration => _factory.Configuration;

    /// <summary>
    /// Creates a new client with updated configuration.
    /// </summary>
    /// <param name="configure">Configuration update action.</param>
    /// <returns>New client instance with updated configuration.</returns>
    public PhotoPrismRestClient WithConfiguration(Action<RestClientConfiguration> configure)
    {
        var newFactory = _factory.WithConfiguration(configure);
        return new PhotoPrismRestClient(newFactory);
    }

    /// <summary>
    /// Creates a new client with different credentials.
    /// </summary>
    /// <param name="username">Username for authentication.</param>
    /// <param name="password">Password for authentication.</param>
    /// <returns>New client instance with updated authentication.</returns>
    public PhotoPrismRestClient WithCredentials(string username, string password)
    {
        var newFactory = _factory.WithCredentials(username, password);
        return new PhotoPrismRestClient(newFactory);
    }

    /// <summary>
    /// Creates a new client with access token authentication.
    /// </summary>
    /// <param name="accessToken">Access token for authentication.</param>
    /// <returns>New client instance with updated authentication.</returns>
    public PhotoPrismRestClient WithAccessToken(string accessToken)
    {
        var newFactory = _factory.WithAccessToken(accessToken);
        return new PhotoPrismRestClient(newFactory);
    }

    /// <summary>
    /// Disposes the client and its resources.
    /// </summary>
    public void Dispose()
    {
        _photos?.Dispose();
        _albums?.Dispose();
        _labels?.Dispose();
        _files?.Dispose();

        if (_disposeFactory)
        {
            // Factory doesn't implement IDisposable, but individual clients do
        }
    }
}
