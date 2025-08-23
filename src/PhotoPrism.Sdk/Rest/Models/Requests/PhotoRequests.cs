namespace PhotoPrism.Sdk.Rest.Models.Requests;

/// <summary>
/// Request parameters for searching photos.
/// </summary>
public class SearchPhotosRequest
{
    /// <summary>
    /// Maximum number of photos to return (1-100000).
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
    /// Whether to group consecutive files that belong to the same photo.
    /// </summary>
    public bool? Merged { get; set; }

    /// <summary>
    /// Whether to exclude private pictures.
    /// </summary>
    public bool? Public { get; set; }

    /// <summary>
    /// Minimum quality score (1-7).
    /// </summary>
    public int? Quality { get; set; }

    /// <summary>
    /// Search query string.
    /// </summary>
    public string? Query { get; set; }

    /// <summary>
    /// Album UID to filter by.
    /// </summary>
    public string? AlbumUid { get; set; }

    /// <summary>
    /// Photo path to filter by.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Whether to filter for video files only.
    /// </summary>
    public bool? Video { get; set; }
}

/// <summary>
/// Request for updating photo metadata.
/// </summary>
public class UpdatePhotoRequest
{
    /// <summary>
    /// Photo title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Photo description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Photo keywords.
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Photo notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether the photo is marked as favorite.
    /// </summary>
    public bool? Favorite { get; set; }

    /// <summary>
    /// Whether the photo is private.
    /// </summary>
    public bool? Private { get; set; }

    /// <summary>
    /// Photo latitude coordinate.
    /// </summary>
    public double? Lat { get; set; }

    /// <summary>
    /// Photo longitude coordinate.
    /// </summary>
    public double? Lng { get; set; }

    /// <summary>
    /// Photo altitude in meters.
    /// </summary>
    public int? Altitude { get; set; }

    /// <summary>
    /// ISO sensitivity.
    /// </summary>
    public int? Iso { get; set; }

    /// <summary>
    /// Focal length in millimeters.
    /// </summary>
    public int? FocalLength { get; set; }

    /// <summary>
    /// F-number (aperture).
    /// </summary>
    public double? FNumber { get; set; }

    /// <summary>
    /// Exposure time.
    /// </summary>
    public string? Exposure { get; set; }

    /// <summary>
    /// Camera ID.
    /// </summary>
    public int? CameraId { get; set; }

    /// <summary>
    /// Lens ID.
    /// </summary>
    public int? LensId { get; set; }

    /// <summary>
    /// Time when the photo was taken.
    /// </summary>
    public DateTime? TakenAt { get; set; }

    /// <summary>
    /// Local time when the photo was taken.
    /// </summary>
    public DateTime? TakenAtLocal { get; set; }

    /// <summary>
    /// Timezone information.
    /// </summary>
    public string? TimeZone { get; set; }
}
