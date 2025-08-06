namespace PhotoPrism.Sdk;

/// <summary>
/// Represents an album in the PhotoPrism system.
/// </summary>
public interface IAlbum
{
    /// <summary>
    /// Database ID of the album.
    /// </summary>
    int ID { get; }

    /// <summary>
    /// Unique identifier for the album.
    /// </summary>
    string? UID { get; }

    /// <summary>
    /// Parent album UID (for nested albums).
    /// </summary>
    string? ParentUID { get; }

    /// <summary>
    /// Album title.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Album caption.
    /// </summary>
    string? Caption { get; }

    /// <summary>
    /// Album description.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Album notes.
    /// </summary>
    string? Notes { get; }

    /// <summary>
    /// Album slug for URLs.
    /// </summary>
    string? Slug { get; }

    /// <summary>
    /// Album path in hierarchy.
    /// </summary>
    string? Path { get; }

    /// <summary>
    /// Album type.
    /// </summary>
    string? Type { get; }

    /// <summary>
    /// Album category.
    /// </summary>
    string? Category { get; }

    /// <summary>
    /// Album template.
    /// </summary>
    string? Template { get; }

    /// <summary>
    /// Album filter criteria.
    /// </summary>
    string? Filter { get; }

    /// <summary>
    /// Album sort order.
    /// </summary>
    string? Order { get; }

    /// <summary>
    /// Album state.
    /// </summary>
    string? State { get; }

    /// <summary>
    /// Country associated with the album.
    /// </summary>
    string? Country { get; }

    /// <summary>
    /// Location associated with the album.
    /// </summary>
    string? Location { get; }

    /// <summary>
    /// Year associated with the album.
    /// </summary>
    int Year { get; }

    /// <summary>
    /// Month associated with the album.
    /// </summary>
    int Month { get; }

    /// <summary>
    /// Day associated with the album.
    /// </summary>
    int Day { get; }

    /// <summary>
    /// Whether the album is marked as favorite.
    /// </summary>
    bool Favorite { get; }

    /// <summary>
    /// Whether the album is private.
    /// </summary>
    bool Private { get; }

    /// <summary>
    /// Thumbnail hash for the album.
    /// </summary>
    string? Thumb { get; }

    /// <summary>
    /// Thumbnail source path.
    /// </summary>
    string? ThumbSrc { get; }

    /// <summary>
    /// User who created the album.
    /// </summary>
    string? CreatedBy { get; }

    /// <summary>
    /// Date and time when the album was created.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the album was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Date and time when the album was deleted (if applicable).
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Date and time when the album was published.
    /// </summary>
    DateTime? PublishedAt { get; }
}
