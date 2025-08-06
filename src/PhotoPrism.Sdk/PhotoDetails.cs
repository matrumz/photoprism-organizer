using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of IPhotoDetails.
/// </summary>
internal record PhotoDetails(
    [property: JsonPropertyName("photoID")] int PhotoID = default,
    [property: JsonPropertyName("Artist")] string? Artist = default,
    [property: JsonPropertyName("ArtistSrc")] string? ArtistSrc = default,
    [property: JsonPropertyName("Copyright")] string? Copyright = default,
    [property: JsonPropertyName("CopyrightSrc")] string? CopyrightSrc = default,
    [property: JsonPropertyName("Keywords")] string? Keywords = default,
    [property: JsonPropertyName("KeywordsSrc")] string? KeywordsSrc = default,
    [property: JsonPropertyName("License")] string? License = default,
    [property: JsonPropertyName("LicenseSrc")] string? LicenseSrc = default,
    [property: JsonPropertyName("Notes")] string? Notes = default,
    [property: JsonPropertyName("NotesSrc")] string? NotesSrc = default,
    [property: JsonPropertyName("Software")] string? Software = default,
    [property: JsonPropertyName("SoftwareSrc")] string? SoftwareSrc = default,
    [property: JsonPropertyName("Subject")] string? Subject = default,
    [property: JsonPropertyName("SubjectSrc")] string? SubjectSrc = default,
    [property: JsonPropertyName("createdAt")] DateTime? CreatedAt = default,
    [property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt = default
) : IPhotoDetails;
