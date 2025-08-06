namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a lens in the PhotoPrism system.
/// </summary>
public interface ILens
{
    /// <summary>
    /// Database ID of the lens.
    /// </summary>
    int ID { get; }

    /// <summary>
    /// Lens manufacturer/make.
    /// </summary>
    string? Make { get; }

    /// <summary>
    /// Lens model.
    /// </summary>
    string? Model { get; }

    /// <summary>
    /// Lens name.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Lens type.
    /// </summary>
    string? Type { get; }

    /// <summary>
    /// Lens description.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Additional notes about the lens.
    /// </summary>
    string? Notes { get; }

    /// <summary>
    /// URL-friendly slug for the lens.
    /// </summary>
    string? Slug { get; }
}
