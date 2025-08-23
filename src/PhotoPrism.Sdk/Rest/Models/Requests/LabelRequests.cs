using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk.Rest.Models.Requests;

/// <summary>
/// Request model for searching labels.
/// </summary>
public class LabelSearchRequest
{
    /// <summary>
    /// Search query text.
    /// </summary>
    [JsonPropertyName("q")]
    public string? Query { get; set; }

    /// <summary>
    /// Maximum number of results to return.
    /// </summary>
    [JsonPropertyName("count")]
    public int Count { get; set; } = 24;

    /// <summary>
    /// Number of results to skip.
    /// </summary>
    [JsonPropertyName("offset")]
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Sort order for results.
    /// </summary>
    [JsonPropertyName("order")]
    public string Order { get; set; } = "name";

    /// <summary>
    /// Whether to show all labels including those with 0 photos.
    /// </summary>
    [JsonPropertyName("all")]
    public bool All { get; set; }
}

/// <summary>
/// Request model for updating a label.
/// </summary>
public class UpdateLabelRequest
{
    /// <summary>
    /// New name for the label.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Priority/importance of the label (-3 to 3).
    /// </summary>
    [JsonPropertyName("priority")]
    public int? Priority { get; set; }
}
