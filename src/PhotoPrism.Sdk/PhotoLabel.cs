using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of IPhotoLabel.
/// </summary>
internal record PhotoLabel(
    [property: JsonPropertyName("photoID")] int PhotoID = default,
    [property: JsonPropertyName("labelID")] int LabelID = default,
    [property: JsonPropertyName("uncertainty")] int Uncertainty = default,
    [property: JsonPropertyName("labelSrc")] string? LabelSrc = default,
    [property: JsonPropertyName("photo")] Photo? Photo = default,
    [property: JsonPropertyName("label")] Label? Label = default
) : IPhotoLabel
{
    IPhoto? IPhotoLabel.Photo => Photo;
    ILabel? IPhotoLabel.Label => Label;
}
