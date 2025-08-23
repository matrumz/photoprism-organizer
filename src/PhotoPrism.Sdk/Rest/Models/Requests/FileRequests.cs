using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk.Rest.Models.Requests;

/// <summary>
/// Request model for changing file orientation.
/// </summary>
public class ChangeFileOrientationRequest
{
    /// <summary>
    /// New orientation for the file (1-8).
    /// </summary>
    [JsonPropertyName("orientation")]
    public int Orientation { get; set; }
}
