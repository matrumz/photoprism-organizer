using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of IPlace.
/// </summary>
internal record Place(
    [property: JsonPropertyName("PlaceID")] string? PlaceID = default,
    [property: JsonPropertyName("Label")] string? Label = default,
    [property: JsonPropertyName("City")] string? City = default,
    [property: JsonPropertyName("State")] string? State = default,
    [property: JsonPropertyName("Country")] string? Country = default,
    [property: JsonPropertyName("District")] string? District = default,
    [property: JsonPropertyName("Keywords")] string? Keywords = default,
    [property: JsonPropertyName("Favorite")] bool Favorite = default,
    [property: JsonPropertyName("PhotoCount")] int PhotoCount = default,
    [property: JsonPropertyName("CreatedAt")] DateTime? CreatedAt = default,
    [property: JsonPropertyName("UpdatedAt")] DateTime? UpdatedAt = default
) : IPlace;
