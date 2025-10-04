namespace PhotoPrism.Sdk;

public partial interface IClient
{
    /// <summary>
    /// Searches for photos using the specified parameters.
    /// </summary>
    /// <param name="count">Maximum number of photos to return (1-100000).</param>
    /// <param name="offset">Search result offset for pagination.</param>
    /// <param name="order">Sort order for results (favorites, name, title, added, edited).</param>
    /// <param name="merged">Whether to group consecutive files that belong to the same photo.</param>
    /// <param name="public">Whether to exclude private pictures.</param>
    /// <param name="quality">Minimum quality score (1-7).</param>
    /// <param name="query">Search query string.</param>
    /// <param name="albumUid">Album UID to filter by.</param>
    /// <param name="path">Photo path to filter by.</param>
    /// <param name="video">Whether to filter for video files only.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of photos matching the search criteria.</returns>
    Task<IReadOnlyList<IPhotoSearchResult>> SearchPhotosAsync(
        int count,
        int offset = 0,
        string? order = null,
        bool? merged = null,
        bool? @public = null,
        int? quality = null,
        string? query = null,
        string? albumUid = null,
        string? path = null,
        bool? video = null,
        CancellationToken cancellationToken = default
    );

    // /// <summary>
    // /// Gets photo details by UID.
    // /// </summary>
    // /// <param name="uid">Photo UID.</param>
    // /// <param name="cancellationToken">Cancellation token.</param>
    // /// <returns>Photo details.</returns>
    // Task<IPhoto?> GetPhotoAsync(string uid, CancellationToken cancellationToken = default);

    // /// <summary>
    // /// Updates photo metadata.
    // /// </summary>
    // /// <param name="uid">Photo UID.</param>
    // /// <param name="photo">Photo data to update.</param>
    // /// <param name="cancellationToken">Cancellation token.</param>
    // /// <returns>Updated photo details.</returns>
    // Task<IPhoto?> UpdatePhotoAsync(string uid, object photo, CancellationToken cancellationToken = default);

    // /// <summary>
    // /// Marks a photo as favorite.
    // /// </summary>
    // /// <param name="uid">Photo UID.</param>
    // /// <param name="cancellationToken">Cancellation token.</param>
    // /// <returns>Task representing the operation.</returns>
    // Task LikePhotoAsync(string uid, CancellationToken cancellationToken = default);

    // /// <summary>
    // /// Removes favorite flag from a photo.
    // /// </summary>
    // /// <param name="uid">Photo UID.</param>
    // /// <param name="cancellationToken">Cancellation token.</param>
    // /// <returns>Task representing the operation.</returns>
    // Task DislikePhotoAsync(string uid, CancellationToken cancellationToken = default);

    // /// <summary>
    // /// Approves a photo that is currently under review.
    // /// </summary>
    // /// <param name="uid">Photo UID.</param>
    // /// <param name="cancellationToken">Cancellation token.</param>
    // /// <returns>Task representing the operation.</returns>
    // Task ApprovePhotoAsync(string uid, CancellationToken cancellationToken = default);

}
