using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of ICell.
/// </summary>
internal record Cell(
    [property: JsonPropertyName("ID")] string? ID = default,
    [property: JsonPropertyName("Name")] string? Name = default,
    [property: JsonPropertyName("Category")] string? Category = default,
    [property: JsonPropertyName("Postcode")] string? Postcode = default,
    [property: JsonPropertyName("Street")] string? Street = default,
    [property: JsonPropertyName("Place")] Place? Place = default,
    [property: JsonPropertyName("CreatedAt")] DateTime? CreatedAt = default,
    [property: JsonPropertyName("UpdatedAt")] DateTime? UpdatedAt = default
) : ICell
{
    IPlace? ICell.Place => Place;
}
