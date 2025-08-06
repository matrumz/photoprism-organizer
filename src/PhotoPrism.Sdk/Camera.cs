using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of ICamera.
/// </summary>
internal record Camera(
    [property: JsonPropertyName("ID")] int ID = default,
    [property: JsonPropertyName("Make")] string? Make = default,
    [property: JsonPropertyName("Model")] string? Model = default,
    [property: JsonPropertyName("Name")] string? Name = default,
    [property: JsonPropertyName("Type")] string? Type = default,
    [property: JsonPropertyName("Description")] string? Description = default,
    [property: JsonPropertyName("Notes")] string? Notes = default,
    [property: JsonPropertyName("Slug")] string? Slug = default
) : ICamera;
