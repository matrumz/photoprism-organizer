namespace PhotoPrism.Sdk.Rest.Models.Requests;

/// <summary>
/// Request parameters for searching albums.
/// </summary>
public class SearchAlbumsRequest
{
    /// <summary>
    /// Maximum number of albums to return (1-100000).
    /// </summary>
    public int Count { get; set; } = 100;

    /// <summary>
    /// Search result offset for pagination.
    /// </summary>
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Sort order for results (favorites, name, title, added, edited).
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Search query string.
    /// </summary>
    public string? Query { get; set; }
}

/// <summary>
/// Request for creating a new album.
/// </summary>
public class CreateAlbumRequest
{
    /// <summary>
    /// Album title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Album description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Album caption.
    /// </summary>
    public string? Caption { get; set; }

    /// <summary>
    /// Album notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether the album is marked as favorite.
    /// </summary>
    public bool Favorite { get; set; } = false;

    /// <summary>
    /// Whether the album is private.
    /// </summary>
    public bool Private { get; set; } = false;

    /// <summary>
    /// Album category.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Album location.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Country associated with the album.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Album type.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Album template.
    /// </summary>
    public string? Template { get; set; }

    /// <summary>
    /// Photo filter for the album.
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Photo order within the album.
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Album thumbnail.
    /// </summary>
    public string? Thumb { get; set; }

    /// <summary>
    /// Album thumbnail source.
    /// </summary>
    public string? ThumbSrc { get; set; }
}

/// <summary>
/// Request for updating album metadata.
/// </summary>
public class UpdateAlbumRequest
{
    /// <summary>
    /// Album title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Album description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Album caption.
    /// </summary>
    public string? Caption { get; set; }

    /// <summary>
    /// Album notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether the album is marked as favorite.
    /// </summary>
    public bool? Favorite { get; set; }

    /// <summary>
    /// Whether the album is private.
    /// </summary>
    public bool? Private { get; set; }

    /// <summary>
    /// Album category.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Album location.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Country associated with the album.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Album type.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Album template.
    /// </summary>
    public string? Template { get; set; }

    /// <summary>
    /// Photo filter for the album.
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Photo order within the album.
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Album thumbnail.
    /// </summary>
    public string? Thumb { get; set; }

    /// <summary>
    /// Album thumbnail source.
    /// </summary>
    public string? ThumbSrc { get; set; }
}

/// <summary>
/// Request for adding photos to an album.
/// </summary>
public class AddPhotosToAlbumRequest
{
    /// <summary>
    /// List of photo UIDs to add to the album.
    /// </summary>
    public required List<string> Photos { get; set; }

    /// <summary>
    /// Whether to add all photos matching criteria.
    /// </summary>
    public bool All { get; set; } = false;
}

/// <summary>
/// Request for removing photos from an album.
/// </summary>
public class RemovePhotosFromAlbumRequest
{
    /// <summary>
    /// List of photo UIDs to remove from the album.
    /// </summary>
    public required List<string> Photos { get; set; }

    /// <summary>
    /// Whether to remove all photos matching criteria.
    /// </summary>
    public bool All { get; set; } = false;
}
