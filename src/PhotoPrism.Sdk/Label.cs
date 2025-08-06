using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of ILabel.
/// </summary>
internal record Label(
    [property: JsonPropertyName("ID")] int ID = default,
    [property: JsonPropertyName("UID")] string? UID = default,
    [property: JsonPropertyName("Name")] string? Name = default,
    [property: JsonPropertyName("Slug")] string? Slug = default,
    [property: JsonPropertyName("CustomSlug")] string? CustomSlug = default,
    [property: JsonPropertyName("Description")] string? Description = default,
    [property: JsonPropertyName("Notes")] string? Notes = default,
    [property: JsonPropertyName("Priority")] int Priority = default,
    [property: JsonPropertyName("Favorite")] bool Favorite = default,
    [property: JsonPropertyName("PhotoCount")] int PhotoCount = default,
    [property: JsonPropertyName("Thumb")] string? Thumb = default,
    [property: JsonPropertyName("ThumbSrc")] string? ThumbSrc = default,
    [property: JsonPropertyName("CreatedAt")] DateTime? CreatedAt = default,
    [property: JsonPropertyName("UpdatedAt")] DateTime? UpdatedAt = default,
    [property: JsonPropertyName("DeletedAt")] DateTime? DeletedAt = default,
    [property: JsonPropertyName("PublishedAt")] DateTime? PublishedAt = default
) : ILabel;
