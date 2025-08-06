namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a camera in the PhotoPrism system.
/// </summary>
public interface ICamera
{
    /// <summary>
    /// Database ID of the camera.
    /// </summary>
    int ID { get; }

    /// <summary>
    /// Camera manufacturer/make.
    /// </summary>
    string? Make { get; }

    /// <summary>
    /// Camera model.
    /// </summary>
    string? Model { get; }

    /// <summary>
    /// Camera name.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Camera type.
    /// </summary>
    string? Type { get; }

    /// <summary>
    /// Camera description.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Additional notes about the camera.
    /// </summary>
    string? Notes { get; }

    /// <summary>
    /// URL-friendly slug for the camera.
    /// </summary>
    string? Slug { get; }
}
