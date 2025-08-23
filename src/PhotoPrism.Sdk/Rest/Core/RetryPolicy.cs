namespace PhotoPrism.Sdk.Rest.Core;

/// <summary>
/// Retry policy configuration and logic.
/// </summary>
public class RetryPolicy
{
    private readonly RestClientConfiguration _configuration;
    private readonly Random _random = new();

    public RetryPolicy(RestClientConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Executes an operation with retry logic.
    /// </summary>
    /// <typeparam name="T">Return type of the operation.</typeparam>
    /// <param name="operation">Operation to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result of the operation.</returns>
    public async Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        var attempt = 0;
        Exception? lastException = null;

        while (attempt <= _configuration.MaxRetryAttempts)
        {
            try
            {
                return await operation(cancellationToken);
            }
            catch (Exception ex) when (attempt < _configuration.MaxRetryAttempts && ShouldRetry(ex, attempt))
            {
                lastException = ex;
                attempt++;

                var delay = CalculateDelay(attempt);
                await Task.Delay(delay, cancellationToken);
            }
        }

        // If we get here, all retry attempts have been exhausted
        throw lastException ?? new InvalidOperationException("Operation failed without an exception");
    }

    /// <summary>
    /// Executes an operation with retry logic (void return).
    /// </summary>
    /// <param name="operation">Operation to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task ExecuteAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(async ct =>
        {
            await operation(ct);
            return true; // Dummy return value
        }, cancellationToken);
    }

    /// <summary>
    /// Determines if an exception should trigger a retry.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="attemptNumber">Current attempt number (1-based).</param>
    /// <returns>True if the operation should be retried.</returns>
    public bool ShouldRetry(Exception exception, int attemptNumber)
    {
        // Don't retry if we've exceeded the maximum attempts
        if (attemptNumber >= _configuration.MaxRetryAttempts)
            return false;

        return exception switch
        {
            // Always retry these
            TimeoutException => true,
            TaskCanceledException when !((TaskCanceledException)exception).CancellationToken.IsCancellationRequested => true,
            HttpRequestException => true,

            // Retry API exceptions if they're marked as retryable
            Exceptions.ApiException apiEx => apiEx.IsRetryable,

            // Don't retry other exceptions
            _ => false
        };
    }

    /// <summary>
    /// Calculates the delay before the next retry attempt.
    /// </summary>
    /// <param name="attemptNumber">Current attempt number (1-based).</param>
    /// <returns>Delay duration.</returns>
    public TimeSpan CalculateDelay(int attemptNumber)
    {
        // Exponential backoff with jitter
        var exponentialDelay = TimeSpan.FromMilliseconds(
            _configuration.RetryBaseDelay.TotalMilliseconds * Math.Pow(2, attemptNumber - 1));

        // Cap at maximum delay
        var cappedDelay = exponentialDelay > _configuration.MaxRetryDelay
            ? _configuration.MaxRetryDelay
            : exponentialDelay;

        // Add jitter (±25% of the delay)
        var jitterRange = cappedDelay.TotalMilliseconds * 0.25;
        var jitter = (_random.NextDouble() - 0.5) * 2 * jitterRange;
        var finalDelay = TimeSpan.FromMilliseconds(cappedDelay.TotalMilliseconds + jitter);

        // Ensure minimum delay
        return finalDelay < TimeSpan.FromMilliseconds(100)
            ? TimeSpan.FromMilliseconds(100)
            : finalDelay;
    }

    /// <summary>
    /// Gets the maximum number of retry attempts.
    /// </summary>
    public int MaxRetryAttempts => _configuration.MaxRetryAttempts;

    /// <summary>
    /// Gets the base delay for retry attempts.
    /// </summary>
    public TimeSpan RetryBaseDelay => _configuration.RetryBaseDelay;

    /// <summary>
    /// Gets the maximum delay between retry attempts.
    /// </summary>
    public TimeSpan MaxRetryDelay => _configuration.MaxRetryDelay;
}
