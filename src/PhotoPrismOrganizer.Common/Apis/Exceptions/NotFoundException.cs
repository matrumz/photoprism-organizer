namespace PhotoPrismOrganizer.Common.Apis.Exceptions;

public class NotFoundException(
    string? message,
    Exception? innerException,
    string? code,
    string? details
) : ApiException(
    message: message,
    innerException: innerException,
    code: code,
    details: details,
    isTransient: false
)
{
}
