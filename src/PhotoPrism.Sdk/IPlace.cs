namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a place in the PhotoPrism system.
/// </summary>
public interface IPlace
{
    /// <summary>
    /// Unique place identifier.
    /// </summary>
    string? PlaceID { get; }

    /// <summary>
    /// Place label/name.
    /// </summary>
    string? Label { get; }

    /// <summary>
    /// City name.
    /// </summary>
    string? City { get; }

    /// <summary>
    /// State/province name.
    /// </summary>
    string? State { get; }

    /// <summary>
    /// Country name.
    /// </summary>
    string? Country { get; }

    /// <summary>
    /// District name.
    /// </summary>
    string? District { get; }

    /// <summary>
    /// Keywords associated with the place.
    /// </summary>
    string? Keywords { get; }

    /// <summary>
    /// Whether the place is marked as favorite.
    /// </summary>
    bool Favorite { get; }

    /// <summary>
    /// Number of photos associated with this place.
    /// </summary>
    int PhotoCount { get; }

    /// <summary>
    /// Date and time when the place was created.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the place was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }
}
