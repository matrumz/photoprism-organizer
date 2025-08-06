namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a geographic cell in the PhotoPrism system.
/// </summary>
public interface ICell
{
    /// <summary>
    /// Unique cell identifier.
    /// </summary>
    string? ID { get; }

    /// <summary>
    /// Cell name.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Cell category.
    /// </summary>
    string? Category { get; }

    /// <summary>
    /// Postal code for the cell.
    /// </summary>
    string? Postcode { get; }

    /// <summary>
    /// Street information for the cell.
    /// </summary>
    string? Street { get; }

    /// <summary>
    /// Associated place information.
    /// </summary>
    IPlace? Place { get; }

    /// <summary>
    /// Date and time when the cell was created.
    /// </summary>
    DateTime? CreatedAt { get; }

    /// <summary>
    /// Date and time when the cell was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }
}
