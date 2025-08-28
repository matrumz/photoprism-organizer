namespace PhotoPrismOrganizer.Common.Apis.Exceptions;

public sealed class RateLimitException(
    string? message,
    Exception? innerException,
    string? code,
    string? details
) : ApiException(
    message: message,
    innerException: innerException,
    code: code,
    details: details,
    isTransient: true
)
{
}
