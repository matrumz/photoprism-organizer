namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a file in the PhotoPrism system.
/// </summary>
public interface IFile
{
    /// <summary>
    /// Unique identifier for the file.
    /// </summary>
    string? UID { get; }

    /// <summary>
    /// Photo UID this file belongs to.
    /// </summary>
    string? PhotoUID { get; }

    /// <summary>
    /// File name.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Original file name.
    /// </summary>
    string? OriginalName { get; }

    /// <summary>
    /// File hash (SHA-1).
    /// </summary>
    string? Hash { get; }

    /// <summary>
    /// File root directory.
    /// </summary>
    string? Root { get; }

    /// <summary>
    /// MIME type of the file.
    /// </summary>
    string? Mime { get; }

    /// <summary>
    /// File type.
    /// </summary>
    string? FileType { get; }

    /// <summary>
    /// Media type.
    /// </summary>
    string? MediaType { get; }

    /// <summary>
    /// Media ID.
    /// </summary>
    string? MediaID { get; }

    /// <summary>
    /// Instance ID.
    /// </summary>
    string? InstanceID { get; }

    /// <summary>
    /// Time index for video files.
    /// </summary>
    string? TimeIndex { get; }

    /// <summary>
    /// File size in bytes.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// File width in pixels.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// File height in pixels.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Aspect ratio.
    /// </summary>
    float AspectRatio { get; }

    /// <summary>
    /// Whether the file is in portrait orientation.
    /// </summary>
    bool Portrait { get; }

    /// <summary>
    /// File orientation (EXIF).
    /// </summary>
    int Orientation { get; }

    /// <summary>
    /// Source of the orientation information.
    /// </summary>
    string? OrientationSrc { get; }

    /// <summary>
    /// Main color of the image.
    /// </summary>
    string? MainColor { get; }

    /// <summary>
    /// Colors present in the image.
    /// </summary>
    string? Colors { get; }

    /// <summary>
    /// Luminance information.
    /// </summary>
    string? Luminance { get; }

    /// <summary>
    /// Chroma value.
    /// </summary>
    int Chroma { get; }

    /// <summary>
    /// Difference value.
    /// </summary>
    int Diff { get; }

    /// <summary>
    /// Whether this is the primary file for the photo.
    /// </summary>
    bool Primary { get; }

    /// <summary>
    /// Whether this is a sidecar file.
    /// </summary>
    bool Sidecar { get; }

    /// <summary>
    /// Whether this is a video file.
    /// </summary>
    bool Video { get; }

    /// <summary>
    /// Whether the file is missing from storage.
    /// </summary>
    bool Missing { get; }

    /// <summary>
    /// Whether the file has HDR.
    /// </summary>
    bool HDR { get; }

    /// <summary>
    /// Whether the file has a watermark.
    /// </summary>
    bool Watermark { get; }

    /// <summary>
    /// Video codec.
    /// </summary>
    string? Codec { get; }

    /// <summary>
    /// Frames per second for video files.
    /// </summary>
    float FPS { get; }

    /// <summary>
    /// Number of frames for video/animated files.
    /// </summary>
    int Frames { get; }

    /// <summary>
    /// Number of pages for document files.
    /// </summary>
    int Pages { get; }

    /// <summary>
    /// Duration for video/audio files.
    /// </summary>
    TimeSpan? Duration { get; }

    /// <summary>
    /// Color profile.
    /// </summary>
    string? ColorProfile { get; }

    /// <summary>
    /// Projection type for 360/VR content.
    /// </summary>
    string? Projection { get; }

    /// <summary>
    /// Software used to create/edit the file.
    /// </summary>
    string? Software { get; }

    /// <summary>
    /// Date and time when the file was taken.
    /// </summary>
    DateTime? TakenAt { get; }

    /// <summary>
    /// Media creation timestamp (UTC).
    /// </summary>
    int MediaUTC { get; }

    /// <summary>
    /// File modification time.
    /// </summary>
    int ModTime { get; }

    /// <summary>
    /// Creation time in processing pipeline.
    /// </summary>
    int CreatedIn { get; }

    /// <summary>
    /// Update time in processing pipeline.
    /// </summary>
    int UpdatedIn { get; }

    /// <summary>
    /// Error message if processing failed.
    /// </summary>
    string? Error { get; }

    /// <summary>
    /// Date and time when the file was created in the system.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the file was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Date and time when the file was deleted (if applicable).
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Date and time when the file was published.
    /// </summary>
    DateTime? PublishedAt { get; }
}
