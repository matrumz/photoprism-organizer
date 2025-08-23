using System.Net;

namespace PhotoPrism.Sdk.Rest.Exceptions;

/// <summary>
/// Base exception for PhotoPrism API-related errors.
/// </summary>
public class ApiException : Exception
{
    /// <summary>
    /// HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Raw response content from the API.
    /// </summary>
    public string? ResponseContent { get; }

    /// <summary>
    /// PhotoPrism error code if available.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Additional error details from the API.
    /// </summary>
    public string? Details { get; }

    /// <summary>
    /// Whether this error is potentially retryable.
    /// </summary>
    public bool IsRetryable { get; }

    public ApiException(string message) : base(message)
    {
        IsRetryable = false;
    }

    public ApiException(string message, Exception innerException) : base(message, innerException)
    {
        IsRetryable = false;
    }

    public ApiException(
        string message,
        HttpStatusCode? statusCode = null,
        string? responseContent = null,
        string? errorCode = null,
        string? details = null,
        bool isRetryable = false,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
        ErrorCode = errorCode;
        Details = details;
        IsRetryable = isRetryable;
    }

    public override string ToString()
    {
        var result = base.ToString();

        if (StatusCode.HasValue)
            result += $"\nHTTP Status Code: {(int)StatusCode.Value} {StatusCode.Value}";

        if (!string.IsNullOrWhiteSpace(ErrorCode))
            result += $"\nError Code: {ErrorCode}";

        if (!string.IsNullOrWhiteSpace(Details))
            result += $"\nDetails: {Details}";

        if (!string.IsNullOrWhiteSpace(ResponseContent))
            result += $"\nResponse Content: {ResponseContent}";

        return result;
    }
}

/// <summary>
/// Exception thrown when authentication fails.
/// </summary>
public class AuthenticationException : ApiException
{
    public AuthenticationException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }

    public AuthenticationException(string message, Exception innerException)
        : base(message, HttpStatusCode.Unauthorized, innerException: innerException)
    {
    }
}

/// <summary>
/// Exception thrown when rate limiting is exceeded.
/// </summary>
public class RateLimitException : ApiException
{
    /// <summary>
    /// When rate limiting will be reset (if available).
    /// </summary>
    public DateTimeOffset? RetryAfter { get; }

    public RateLimitException(string message, DateTimeOffset? retryAfter = null)
        : base(message, HttpStatusCode.TooManyRequests, isRetryable: true)
    {
        RetryAfter = retryAfter;
    }
}

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : ApiException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string resourceType, string resourceId)
        : base($"{resourceType} with ID '{resourceId}' was not found", HttpStatusCode.NotFound)
    {
    }
}

/// <summary>
/// Exception thrown when the request is invalid or malformed.
/// </summary>
public class BadRequestException : ApiException
{
    public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(string message, string? details)
        : base(message, HttpStatusCode.BadRequest, details: details)
    {
    }
}

/// <summary>
/// Exception thrown when access to a resource is forbidden.
/// </summary>
public class ForbiddenException : ApiException
{
    public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }
}

/// <summary>
/// Exception thrown when a server error occurs.
/// </summary>
public class ServerException : ApiException
{
    public ServerException(string message, HttpStatusCode statusCode)
        : base(message, statusCode, isRetryable: true)
    {
    }

    public ServerException(string message, HttpStatusCode statusCode, string? responseContent)
        : base(message, statusCode, responseContent, isRetryable: true)
    {
    }
}
