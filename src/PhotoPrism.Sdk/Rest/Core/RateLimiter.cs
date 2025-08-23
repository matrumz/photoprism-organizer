namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// Token bucket rate limiter implementation.
/// </summary>
public class RateLimiter
{
    private readonly int _capacity;
    private readonly int _refillRate; // tokens per second
    private readonly object _lock = new();
    private int _tokens;
    private DateTime _lastRefill;

    /// <summary>
    /// Creates a new rate limiter.
    /// </summary>
    /// <param name="requestsPerMinute">Maximum requests per minute (0 = unlimited).</param>
    public RateLimiter(int requestsPerMinute)
    {
        if (requestsPerMinute <= 0)
        {
            _capacity = int.MaxValue;
            _refillRate = int.MaxValue;
        }
        else
        {
            _capacity = Math.Max(1, requestsPerMinute / 6); // Allow bursts up to 10 seconds worth
            _refillRate = Math.Max(1, requestsPerMinute / 60); // tokens per second
        }

        _tokens = _capacity;
        _lastRefill = DateTime.UtcNow;
    }

    /// <summary>
    /// Attempts to acquire a token for making a request.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if a token was acquired, false if rate limited.</returns>
    public async Task<bool> TryAcquireAsync(CancellationToken cancellationToken = default)
    {
        if (_capacity == int.MaxValue)
            return true; // No rate limiting

        lock (_lock)
        {
            RefillTokens();

            if (_tokens > 0)
            {
                _tokens--;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Waits until a token becomes available.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task that completes when a token is acquired.</returns>
    public async Task AcquireAsync(CancellationToken cancellationToken = default)
    {
        if (_capacity == int.MaxValue)
            return; // No rate limiting

        while (!await TryAcquireAsync(cancellationToken))
        {
            // Calculate how long to wait for the next token
            var waitTime = CalculateWaitTime();
            await Task.Delay(waitTime, cancellationToken);
        }
    }

    /// <summary>
    /// Gets the estimated time until the next token becomes available.
    /// </summary>
    /// <returns>Wait time for the next available token.</returns>
    public TimeSpan GetEstimatedWaitTime()
    {
        if (_capacity == int.MaxValue)
            return TimeSpan.Zero;

        lock (_lock)
        {
            RefillTokens();

            if (_tokens > 0)
                return TimeSpan.Zero;

            return CalculateWaitTime();
        }
    }

    private void RefillTokens()
    {
        var now = DateTime.UtcNow;
        var elapsed = now - _lastRefill;

        if (elapsed.TotalSeconds >= 1.0)
        {
            var tokensToAdd = (int)(elapsed.TotalSeconds * _refillRate);
            _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
            _lastRefill = now;
        }
    }

    private TimeSpan CalculateWaitTime()
    {
        // Wait time for one token to be refilled
        return TimeSpan.FromSeconds(1.0 / _refillRate);
    }

    /// <summary>
    /// Gets the current number of available tokens.
    /// </summary>
    public int AvailableTokens
    {
        get
        {
            lock (_lock)
            {
                RefillTokens();
                return _tokens;
            }
        }
    }

    /// <summary>
    /// Gets the maximum capacity of the token bucket.
    /// </summary>
    public int Capacity => _capacity == int.MaxValue ? 0 : _capacity;
}
