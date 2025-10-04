namespace PhotoPrism.Sdk;

/// <summary>
/// Represents photo details/metadata in the PhotoPrism system.
/// </summary>
public interface IPhotoDetails
{
    /// <summary>
    /// Photo ID this details record belongs to.
    /// </summary>
    int PhotoID { get; }

    /// <summary>
    /// Artist information.
    /// </summary>
    string? Artist { get; }

    /// <summary>
    /// Source of the artist information.
    /// </summary>
    string? ArtistSrc { get; }

    /// <summary>
    /// Copyright information.
    /// </summary>
    string? Copyright { get; }

    /// <summary>
    /// Source of the copyright information.
    /// </summary>
    string? CopyrightSrc { get; }

    /// <summary>
    /// Keywords associated with the photo.
    /// </summary>
    string? Keywords { get; }

    /// <summary>
    /// Source of the keywords information.
    /// </summary>
    string? KeywordsSrc { get; }

    /// <summary>
    /// License information.
    /// </summary>
    string? License { get; }

    /// <summary>
    /// Source of the license information.
    /// </summary>
    string? LicenseSrc { get; }

    /// <summary>
    /// Additional notes about the photo.
    /// </summary>
    string? Notes { get; }

    /// <summary>
    /// Source of the notes information.
    /// </summary>
    string? NotesSrc { get; }

    /// <summary>
    /// Software used to process the photo.
    /// </summary>
    string? Software { get; }

    /// <summary>
    /// Source of the software information.
    /// </summary>
    string? SoftwareSrc { get; }

    /// <summary>
    /// Subject of the photo.
    /// </summary>
    string? Subject { get; }

    /// <summary>
    /// Source of the subject information.
    /// </summary>
    string? SubjectSrc { get; }

    /// <summary>
    /// Date and time when the details were created.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the details were last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }
}
