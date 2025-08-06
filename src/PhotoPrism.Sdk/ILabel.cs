namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a label in the PhotoPrism system.
/// </summary>
public interface ILabel
{
    /// <summary>
    /// Database ID of the label.
    /// </summary>
    int ID { get; }

    /// <summary>
    /// Unique identifier for the label.
    /// </summary>
    string? UID { get; }

    /// <summary>
    /// Label name.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// URL-friendly slug for the label.
    /// </summary>
    string? Slug { get; }

    /// <summary>
    /// Custom slug for the label.
    /// </summary>
    string? CustomSlug { get; }

    /// <summary>
    /// Label description.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Additional notes about the label.
    /// </summary>
    string? Notes { get; }

    /// <summary>
    /// Priority/importance of the label.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Whether the label is marked as favorite.
    /// </summary>
    bool Favorite { get; }

    /// <summary>
    /// Number of photos associated with this label.
    /// </summary>
    int PhotoCount { get; }

    /// <summary>
    /// Thumbnail hash for the label.
    /// </summary>
    string? Thumb { get; }

    /// <summary>
    /// Thumbnail source path.
    /// </summary>
    string? ThumbSrc { get; }

    /// <summary>
    /// Date and time when the label was created.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the label was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Date and time when the label was deleted (if applicable).
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Date and time when the label was published.
    /// </summary>
    DateTime? PublishedAt { get; }
}
