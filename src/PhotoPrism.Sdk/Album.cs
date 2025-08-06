using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of IAlbum.
/// </summary>
internal record Album(
    [property: JsonPropertyName("ID")] int ID = default,
    [property: JsonPropertyName("UID")] string? UID = default,
    [property: JsonPropertyName("ParentUID")] string? ParentUID = default,
    [property: JsonPropertyName("Title")] string? Title = default,
    [property: JsonPropertyName("Caption")] string? Caption = default,
    [property: JsonPropertyName("Description")] string? Description = default,
    [property: JsonPropertyName("Notes")] string? Notes = default,
    [property: JsonPropertyName("Slug")] string? Slug = default,
    [property: JsonPropertyName("Path")] string? Path = default,
    [property: JsonPropertyName("Type")] string? Type = default,
    [property: JsonPropertyName("Category")] string? Category = default,
    [property: JsonPropertyName("Template")] string? Template = default,
    [property: JsonPropertyName("Filter")] string? Filter = default,
    [property: JsonPropertyName("Order")] string? Order = default,
    [property: JsonPropertyName("State")] string? State = default,
    [property: JsonPropertyName("Country")] string? Country = default,
    [property: JsonPropertyName("Location")] string? Location = default,
    [property: JsonPropertyName("Year")] int Year = default,
    [property: JsonPropertyName("Month")] int Month = default,
    [property: JsonPropertyName("Day")] int Day = default,
    [property: JsonPropertyName("Favorite")] bool Favorite = default,
    [property: JsonPropertyName("Private")] bool Private = default,
    [property: JsonPropertyName("Thumb")] string? Thumb = default,
    [property: JsonPropertyName("ThumbSrc")] string? ThumbSrc = default,
    [property: JsonPropertyName("CreatedBy")] string? CreatedBy = default,
    [property: JsonPropertyName("CreatedAt")] DateTime? CreatedAt = default,
    [property: JsonPropertyName("UpdatedAt")] DateTime? UpdatedAt = default,
    [property: JsonPropertyName("DeletedAt")] DateTime? DeletedAt = default,
    [property: JsonPropertyName("PublishedAt")] DateTime? PublishedAt = default
) : IAlbum;
