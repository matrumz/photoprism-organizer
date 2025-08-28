namespace PhotoPrismOrganizer.Common.Apis.Exceptions;

public sealed class UnauthenticatedException(
    string? message,
    Exception? innerException,
    string? code,
    string? details
) : AccessException(
    message: message,
    innerException: innerException,
    code: code,
    details: details
)
{
}
