namespace PhotoPrismOrganizer.Common.Apis.Exceptions;

public class ApiException(
    string? message,
    Exception? innerException,
    string? code,
    string? details,
    bool isTransient = false
) : Exception(message, innerException)
{

    /// <summary>
    /// API status code.
    /// </summary>
    public string? Code => code;

    /// <summary>
    /// API status details.
    /// </summary>
    public string? Details => details;

    /// <summary>
    /// Indicates whether the error is transient.
    /// </summary>
    /// <remarks>
    /// Transient errors are temporary and can be retried.
    /// </remarks>
    public bool IsTransient => isTransient;

}
