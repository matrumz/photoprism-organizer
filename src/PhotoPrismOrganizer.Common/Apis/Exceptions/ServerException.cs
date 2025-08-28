namespace PhotoPrismOrganizer.Common.Apis.Exceptions;

public class ServerException(
    string? message,
    Exception? innerException,
    string? code,
    string? details,
    bool isTransient = true
) : ApiException(
    message: message,
    innerException: innerException,
    code: code,
    details: details,
    isTransient: isTransient
)
{
}
