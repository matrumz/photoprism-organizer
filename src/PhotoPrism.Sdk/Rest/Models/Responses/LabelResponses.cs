using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk.Rest.Models.Responses;

/// <summary>
/// Represents a label search response from the PhotoPrism API.
/// </summary>
public class LabelSearchResponse
{
    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("priority")]
    public int Priority { get; set; }

    [JsonPropertyName("favorite")]
    public bool IsFavorite { get; set; }

    [JsonPropertyName("photoCount")]
    public int PhotoCount { get; set; }

    [JsonPropertyName("thumb")]
    public string? Thumb { get; set; }

    [JsonPropertyName("thumbSrc")]
    public string? ThumbSrc { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
