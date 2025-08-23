using System.Collections.Concurrent;

namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// In-memory cache for API responses.
/// </summary>
public class CacheManager
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly Timer _cleanupTimer;
    private readonly TimeSpan _defaultTtl;

    public CacheManager(TimeSpan defaultTtl)
    {
        _defaultTtl = defaultTtl;
        // Run cleanup every minute
        _cleanupTimer = new Timer(Cleanup, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Gets a cached value if it exists and hasn't expired.
    /// </summary>
    /// <typeparam name="T">Type of the cached value.</typeparam>
    /// <param name="key">Cache key.</param>
    /// <returns>Cached value or null if not found or expired.</returns>
    public T? Get<T>(string key) where T : class
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt > DateTimeOffset.UtcNow)
            {
                return entry.Value as T;
            }
            else
            {
                // Remove expired entry
                _cache.TryRemove(key, out _);
            }
        }

        return null;
    }

    /// <summary>
    /// Sets a value in the cache with the default TTL.
    /// </summary>
    /// <param name="key">Cache key.</param>
    /// <param name="value">Value to cache.</param>
    public void Set(string key, object value)
    {
        Set(key, value, _defaultTtl);
    }

    /// <summary>
    /// Sets a value in the cache with a specific TTL.
    /// </summary>
    /// <param name="key">Cache key.</param>
    /// <param name="value">Value to cache.</param>
    /// <param name="ttl">Time to live.</param>
    public void Set(string key, object value, TimeSpan ttl)
    {
        var entry = new CacheEntry
        {
            Value = value,
            ExpiresAt = DateTimeOffset.UtcNow.Add(ttl)
        };

        _cache.AddOrUpdate(key, entry, (_, _) => entry);
    }

    /// <summary>
    /// Removes a specific key from the cache.
    /// </summary>
    /// <param name="key">Cache key to remove.</param>
    public void Remove(string key)
    {
        _cache.TryRemove(key, out _);
    }

    /// <summary>
    /// Removes all keys that match a pattern.
    /// </summary>
    /// <param name="pattern">Pattern to match (supports * wildcard).</param>
    public void RemoveByPattern(string pattern)
    {
        var regex = new System.Text.RegularExpressions.Regex(
            "^" + pattern.Replace("*", ".*") + "$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        var keysToRemove = _cache.Keys.Where(key => regex.IsMatch(key)).ToList();

        foreach (var key in keysToRemove)
        {
            _cache.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Clears all cached entries.
    /// </summary>
    public void Clear()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Gets or sets a cached value using the provided factory function.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="key">Cache key.</param>
    /// <param name="factory">Factory function to create the value if not cached.</param>
    /// <param name="ttl">Time to live (optional, uses default if not specified).</param>
    /// <returns>Cached or newly created value.</returns>
    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null) where T : class
    {
        var cached = Get<T>(key);
        if (cached != null)
            return cached;

        var value = await factory();
        if (value != null)
        {
            Set(key, value, ttl ?? _defaultTtl);
        }

        return value;
    }

    /// <summary>
    /// Gets cache statistics.
    /// </summary>
    /// <returns>Cache statistics.</returns>
    public CacheStatistics GetStatistics()
    {
        var now = DateTimeOffset.UtcNow;
        var entries = _cache.Values.ToList();

        return new CacheStatistics
        {
            TotalEntries = entries.Count,
            ExpiredEntries = entries.Count(e => e.ExpiresAt <= now),
            ValidEntries = entries.Count(e => e.ExpiresAt > now)
        };
    }

    private void Cleanup(object? state)
    {
        var now = DateTimeOffset.UtcNow;
        var expiredKeys = _cache
            .Where(kvp => kvp.Value.ExpiresAt <= now)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in expiredKeys)
        {
            _cache.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Disposes the cache manager and cleanup timer.
    /// </summary>
    public void Dispose()
    {
        _cleanupTimer?.Dispose();
        _cache.Clear();
    }

    private class CacheEntry
    {
        public required object Value { get; init; }
        public required DateTimeOffset ExpiresAt { get; init; }
    }
}

/// <summary>
/// Cache statistics information.
/// </summary>
public class CacheStatistics
{
    /// <summary>
    /// Total number of entries in the cache.
    /// </summary>
    public int TotalEntries { get; init; }

    /// <summary>
    /// Number of expired entries.
    /// </summary>
    public int ExpiredEntries { get; init; }

    /// <summary>
    /// Number of valid (non-expired) entries.
    /// </summary>
    public int ValidEntries { get; init; }

    /// <summary>
    /// Cache hit ratio (0.0 to 1.0).
    /// </summary>
    public double HitRatio => TotalEntries > 0 ? (double)ValidEntries / TotalEntries : 0.0;
}
