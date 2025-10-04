namespace PhotoPrism.Sdk;

/// <summary>
/// Represents a photo label association in the PhotoPrism system.
/// </summary>
public interface IPhotoLabel
{
    /// <summary>
    /// Photo ID this label is associated with.
    /// </summary>
    int PhotoID { get; }

    /// <summary>
    /// Label ID.
    /// </summary>
    int LabelID { get; }

    /// <summary>
    /// Uncertainty/confidence level of the label (0-100).
    /// </summary>
    int Uncertainty { get; }

    /// <summary>
    /// Source of the label information.
    /// </summary>
    string? LabelSrc { get; }

    /// <summary>
    /// Associated photo information.
    /// </summary>
    IPhoto? Photo { get; }

    /// <summary>
    /// Associated label information.
    /// </summary>
    ILabel? Label { get; }
}
