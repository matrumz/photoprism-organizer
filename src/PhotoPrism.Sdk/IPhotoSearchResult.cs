namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a photo search result in the PhotoPrism system.
/// </summary>
public interface IPhotoSearchResult
{
    /// <summary>
    /// Unique identifier for the photo.
    /// </summary>
    string? UID { get; }

    /// <summary>
    /// Database ID of the photo.
    /// </summary>
    string? ID { get; }

    /// <summary>
    /// Title of the photo.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Caption/description of the photo.
    /// </summary>
    string? Caption { get; }

    /// <summary>
    /// Detailed description of the photo.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Original filename of the photo.
    /// </summary>
    string? OriginalName { get; }

    /// <summary>
    /// Current filename/name of the photo.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// File path of the photo.
    /// </summary>
    string? Path { get; }

    /// <summary>
    /// Type of the photo (image, video, etc.).
    /// </summary>
    string? Type { get; }

    /// <summary>
    /// Source of the type information.
    /// </summary>
    string? TypeSrc { get; }

    /// <summary>
    /// Date and time when the photo was taken (UTC).
    /// </summary>
    DateTime? TakenAt { get; }

    /// <summary>
    /// Date and time when the photo was taken (local time).
    /// </summary>
    DateTime? TakenAtLocal { get; }

    /// <summary>
    /// Source of the taken date information.
    /// </summary>
    string? TakenSrc { get; }

    /// <summary>
    /// Timezone where the photo was taken.
    /// </summary>
    string? TimeZone { get; }

    /// <summary>
    /// Year when the photo was taken.
    /// </summary>
    int Year { get; }

    /// <summary>
    /// Month when the photo was taken.
    /// </summary>
    int Month { get; }

    /// <summary>
    /// Day when the photo was taken.
    /// </summary>
    int Day { get; }

    /// <summary>
    /// Latitude coordinate.
    /// </summary>
    double? Lat { get; }

    /// <summary>
    /// Longitude coordinate.
    /// </summary>
    double? Lng { get; }

    /// <summary>
    /// Altitude in meters.
    /// </summary>
    int Altitude { get; }

    /// <summary>
    /// Country where the photo was taken.
    /// </summary>
    string? Country { get; }

    /// <summary>
    /// Place ID where the photo was taken.
    /// </summary>
    string? PlaceID { get; }

    /// <summary>
    /// Source of the place information.
    /// </summary>
    string? PlaceSrc { get; }

    /// <summary>
    /// Cell ID for location data.
    /// </summary>
    string? CellID { get; }

    /// <summary>
    /// Accuracy of the cell location data.
    /// </summary>
    int CellAccuracy { get; }

    /// <summary>
    /// Camera ID used to take the photo.
    /// </summary>
    int CameraID { get; }

    /// <summary>
    /// Camera make/manufacturer.
    /// </summary>
    string? CameraMake { get; }

    /// <summary>
    /// Camera model.
    /// </summary>
    string? CameraModel { get; }

    /// <summary>
    /// Camera serial number.
    /// </summary>
    string? CameraSerial { get; }

    /// <summary>
    /// Source of the camera information.
    /// </summary>
    string? CameraSrc { get; }

    /// <summary>
    /// Camera type.
    /// </summary>
    string? CameraType { get; }

    /// <summary>
    /// Lens ID used to take the photo.
    /// </summary>
    int LensID { get; }

    /// <summary>
    /// Lens make/manufacturer.
    /// </summary>
    string? LensMake { get; }

    /// <summary>
    /// Lens model.
    /// </summary>
    string? LensModel { get; }

    /// <summary>
    /// Focal length in millimeters.
    /// </summary>
    int FocalLength { get; }

    /// <summary>
    /// F-number (aperture).
    /// </summary>
    float FNumber { get; }

    /// <summary>
    /// ISO sensitivity.
    /// </summary>
    int Iso { get; }

    /// <summary>
    /// Exposure time.
    /// </summary>
    string? Exposure { get; }

    /// <summary>
    /// Quality score (1-7).
    /// </summary>
    int Quality { get; }

    /// <summary>
    /// Resolution of the photo.
    /// </summary>
    int Resolution { get; }

    /// <summary>
    /// Color value/score.
    /// </summary>
    int Color { get; }

    /// <summary>
    /// Duration for video files.
    /// </summary>
    TimeSpan? Duration { get; }

    /// <summary>
    /// Number of detected faces.
    /// </summary>
    int Faces { get; }

    /// <summary>
    /// Whether the photo is marked as favorite.
    /// </summary>
    bool Favorite { get; }

    /// <summary>
    /// Whether the photo is private.
    /// </summary>
    bool Private { get; }

    /// <summary>
    /// Whether the photo is a scan.
    /// </summary>
    bool Scan { get; }

    /// <summary>
    /// Whether the photo is a panorama.
    /// </summary>
    bool Panorama { get; }

    /// <summary>
    /// Whether the photo is in portrait orientation.
    /// </summary>
    bool Portrait { get; }

    /// <summary>
    /// Stack number for grouped photos.
    /// </summary>
    int Stack { get; }

    /// <summary>
    /// Document ID for identification.
    /// </summary>
    string? DocumentID { get; }

    /// <summary>
    /// Instance ID.
    /// </summary>
    string? InstanceID { get; }

    /// <summary>
    /// File hash.
    /// </summary>
    string? Hash { get; }

    /// <summary>
    /// File width in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// File height in pixels.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// File UID.
    /// </summary>
    string? FileUID { get; }

    /// <summary>
    /// File name.
    /// </summary>
    string? FileName { get; }

    /// <summary>
    /// File root directory.
    /// </summary>
    string? FileRoot { get; }

    /// <summary>
    /// Whether search results are merged.
    /// </summary>
    bool Merged { get; }

    /// <summary>
    /// Place city.
    /// </summary>
    string? PlaceCity { get; }

    /// <summary>
    /// Place state.
    /// </summary>
    string? PlaceState { get; }

    /// <summary>
    /// Place country.
    /// </summary>
    string? PlaceCountry { get; }

    /// <summary>
    /// Place label.
    /// </summary>
    string? PlaceLabel { get; }

    /// <summary>
    /// Details artist.
    /// </summary>
    string? DetailsArtist { get; }

    /// <summary>
    /// Details copyright.
    /// </summary>
    string? DetailsCopyright { get; }

    /// <summary>
    /// Details keywords.
    /// </summary>
    string? DetailsKeywords { get; }

    /// <summary>
    /// Details license.
    /// </summary>
    string? DetailsLicense { get; }

    /// <summary>
    /// Details subject.
    /// </summary>
    string? DetailsSubject { get; }

    /// <summary>
    /// Date and time when the photo was created in the system.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the photo was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Date and time when the photo was deleted (if applicable).
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Date and time when the photo was last edited.
    /// </summary>
    DateTime? EditedAt { get; }

    /// <summary>
    /// Date and time when the photo was last checked.
    /// </summary>
    DateTime? CheckedAt { get; }

    /// <summary>
    /// Associated files for this photo (if search results are merged).
    /// </summary>
    IReadOnlyList<IFile>? Files { get; }
}
