using System.Net;

namespace PhotoPrismOrganizer.Common.Apis.Exceptions;

/// <summary>
/// Factory for creating appropriate API exceptions based on HTTP status codes.
/// </summary>
public static class ApiExceptionFactory
{

    private static readonly Dictionary<HttpStatusCode, Func<string, string, ApiException>> HttpExceptionMap = new()
    {

        [HttpStatusCode.BadRequest] = (context, content) => new BadRequestException(
            message: $"Invalid parameters provided for {context}",
            innerException: null,
            code: HttpStatusCode.BadRequest.ToString(),
            details: content
        ),

        [HttpStatusCode.Unauthorized] = (context, content) => new UnauthenticatedException(
            message: $"Authentication required for {context}",
            innerException: null,
            code: HttpStatusCode.Unauthorized.ToString(),
            details: content
        ),

        [HttpStatusCode.Forbidden] = (context, content) => new UnauthorizedException(
            message: $"Insufficient permissions for {context}",
            innerException: null,
            code: HttpStatusCode.Forbidden.ToString(),
            details: content
        ),

        [HttpStatusCode.NotFound] = (context, content) => new NotFoundException(
            message: $"Resource not found for {context}",
            innerException: null,
            code: HttpStatusCode.NotFound.ToString(),
            details: content
        ),

        [HttpStatusCode.TooManyRequests] = (context, content) => new RateLimitException(
            message: $"Rate limit exceeded for {context}",
            innerException: null,
            code: HttpStatusCode.TooManyRequests.ToString(),
            details: content
        )

    };

    /// <summary>
    /// Creates an appropriate exception based on the HTTP status code and operation context.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="content">The response content.</param>
    /// <param name="operationContext">The operation context (e.g., "photo search").</param>
    /// <returns>An appropriate exception for the status code.</returns>
    public static ApiException CreateHttpException(HttpStatusCode statusCode, string content, string operationContext)
    {
        // Handle mapped status codes
        if (HttpExceptionMap.TryGetValue(statusCode, out var factory))
        {
            return factory(operationContext, content);
        }

        // Handle server errors (5xx)
        if ((int)statusCode >= 500)
        {
            return new ServerException(
                message: $"Server error occurred during {operationContext}",
                innerException: null,
                code: statusCode.ToString(),
                details: content
            );
        }

        // Default case for unmapped status codes
        return new ApiException(
            message: $"Unexpected HTTP status {statusCode} for {operationContext}",
            innerException: null,
            code: statusCode.ToString(),
            details: content
        );
    }

    /// <summary>
    /// Creates a network-related exception.
    /// </summary>
    public static ApiException CreateNetworkException(string operationContext, HttpRequestException innerException) =>
        new(
            message: $"Network error occurred during {operationContext}",
            innerException: innerException,
            code: "NETWORK_ERROR",
            details: innerException.Message,
            isTransient: true
        );

    /// <summary>
    /// Creates a timeout exception.
    /// </summary>
    public static ApiException CreateTimeoutException(string operationContext, TaskCanceledException innerException) =>
        new(
            message: $"Request timeout during {operationContext}",
            innerException: innerException,
            code: "TIMEOUT",
            details: "The request took too long to complete",
            isTransient: true
        );

    /// <summary>
    /// Creates a cancellation exception.
    /// </summary>
    public static ApiException CreateCancellationException(string operationContext, TaskCanceledException innerException) =>
        new(
            message: $"Operation {operationContext} was cancelled",
            innerException: innerException,
            code: "CANCELLED",
            details: "The operation was cancelled by the caller"
        );

    /// <summary>
    /// Creates a JSON parsing exception.
    /// </summary>
    public static ApiException CreateJsonException(string operationContext, System.Text.Json.JsonException? innerException) =>
        new(
            message: $"Failed to parse response for {operationContext}",
            innerException: innerException,
            code: "JSON_PARSE_ERROR",
            details: innerException?.Message
        );

}
