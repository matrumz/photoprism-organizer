using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PhotoPrism.Sdk.Hosting;
using PhotoPrism.Sdk.Rest.V1;

using PhotoPrismOrganizer.Common;
using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk;

public class ClientFactory : IClientFactory, IClientFactoryManager, IDisposable
{
    private readonly ILogger<ClientFactory> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<PhotoPrismSdkOptions> _optionsMonitor;
    private readonly IDisposable? _optionsChangeToken;

    private readonly ConcurrentDictionary<string, Lazy<IClient>> _clients = new();
    private readonly ConcurrentDictionary<string, PhotoPrismSdkOptions.InstanceOptions> _instanceOptionsSnapshot = new();

    private string? _activeInstanceKey;
    private bool _disposed;

    public ClientFactory(
        ILogger<ClientFactory> logger,
        ILoggerFactory loggerFactory,
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<PhotoPrismSdkOptions> optionsMonitor)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _httpClientFactory = httpClientFactory;
        _optionsMonitor = optionsMonitor;

        _optionsChangeToken = _optionsMonitor.OnChange(OnOptionsChanged);
    }

    IClient IClientFactory.CreateClient()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (string.IsNullOrEmpty(_activeInstanceKey))
        {
            throw new InvalidOperationException(
                "No active PhotoPrism instance has been set. " +
                "Call SetActiveInstance(instanceKey) first, or use CreateClient(instanceKey) to specify an instance explicitly.");
        }

        return ((IClientFactory)this).CreateClient(_activeInstanceKey);
    }

    IClient IClientFactory.CreateClient(string instanceKey)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceKey);

        var client = _clients.GetOrAdd(instanceKey, key => new Lazy<IClient>(() => BuildClient(key)));
        return client.Value;
    }

    IClient IClientFactoryManager.SetActiveInstance(string instanceKey)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(instanceKey);

        var options = _optionsMonitor.CurrentValue;
        if (!options.Instances.ContainsKey(instanceKey))
        {
            throw new ArgumentException(
                $"PhotoPrism instance '{instanceKey}' is not configured. " +
                $"Available instances: {string.Join(", ", options.Instances.Keys)}",
                nameof(instanceKey));
        }

        _activeInstanceKey = instanceKey;
        _logger.LogInformation("Active PhotoPrism instance set to '{InstanceKey}'", instanceKey);

        // Pre-warm the cache by creating the client
        return ((IClientFactory)this).CreateClient(instanceKey);
    }

    private IClient BuildClient(string instanceKey)
    {
        var options = _optionsMonitor.CurrentValue;

        if (!options.Instances.TryGetValue(instanceKey, out var instanceOptions))
        {
            throw new ArgumentException(
                $"PhotoPrism instance '{instanceKey}' is not configured. " +
                $"Available instances: {string.Join(", ", options.Instances.Keys)}",
                nameof(instanceKey));
        }

        // Snapshot the options for change detection later
        _instanceOptionsSnapshot[instanceKey] = instanceOptions;

        // Parse version, defaulting to a high version for "latest" to enable all features
        var version = instanceOptions.Version.Equals("latest", StringComparison.OrdinalIgnoreCase)
            ? new SemanticVersion(999, 999, 999)
            : SemanticVersion.Parse(instanceOptions.Version, "1.0.0");

        // Create HttpClient and configure BaseAddress dynamically
        var httpClient = _httpClientFactory.CreateClient(instanceKey);
        httpClient.BaseAddress = new Uri(instanceOptions.Url);

        // Build REST clients
        var photosRestClient = new PhotosRestClient(
            httpClient,
            _loggerFactory.CreateLogger<PhotosRestClient>());

        // Build operation registry with all REST clients
        var operationRegistry = new OperationRegistry(version, [photosRestClient]);

        // Build and return the client
        var client = new Client(
            _loggerFactory.CreateLogger<Client>(),
            operationRegistry);

        _logger.LogDebug(
            "Created PhotoPrism client for instance '{InstanceKey}' at '{Url}' (version: {Version})",
            instanceKey,
            instanceOptions.Url,
            instanceOptions.Version);

        return client;
    }

    private void OnOptionsChanged(PhotoPrismSdkOptions newOptions, string? name)
    {
        // Identify clients that need to be invalidated
        var keysToRemove = new List<string>();

        foreach (var (instanceKey, _) in _clients)
        {
            // Check if instance was removed from configuration
            if (!newOptions.Instances.TryGetValue(instanceKey, out var newInstanceOptions))
            {
                _logger.LogInformation(
                    "PhotoPrism instance '{InstanceKey}' was removed from configuration, invalidating client",
                    instanceKey);
                keysToRemove.Add(instanceKey);
                continue;
            }

            // Check if instance configuration changed
            if (_instanceOptionsSnapshot.TryGetValue(instanceKey, out var oldInstanceOptions))
            {
                if (oldInstanceOptions.Url != newInstanceOptions.Url ||
                    oldInstanceOptions.Version != newInstanceOptions.Version)
                {
                    _logger.LogInformation(
                        "PhotoPrism instance '{InstanceKey}' configuration changed, invalidating client",
                        instanceKey);
                    keysToRemove.Add(instanceKey);
                }
            }
        }

        // Remove invalidated clients
        foreach (var key in keysToRemove)
        {
            _clients.TryRemove(key, out _);
            _instanceOptionsSnapshot.TryRemove(key, out _);
        }

        // Clear active instance if it was removed
        if (_activeInstanceKey != null && !newOptions.Instances.ContainsKey(_activeInstanceKey))
        {
            _logger.LogWarning(
                "Active PhotoPrism instance '{InstanceKey}' was removed from configuration",
                _activeInstanceKey);
            _activeInstanceKey = null;
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _optionsChangeToken?.Dispose();
        _clients.Clear();
        _instanceOptionsSnapshot.Clear();

        GC.SuppressFinalize(this);
    }
}
