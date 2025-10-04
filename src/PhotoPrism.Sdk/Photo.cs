using System.Text.Json.Serialization;

namespace PhotoPrism.Sdk;

/// <summary>
/// Internal record implementation of IPhoto.
/// </summary>
internal record Photo(
    [property: JsonPropertyName("UID")] string? UID = default,
    [property: JsonPropertyName("ID")] int ID = default,
    [property: JsonPropertyName("Title")] string? Title = default,
    [property: JsonPropertyName("TitleSrc")] string? TitleSrc = default,
    [property: JsonPropertyName("Caption")] string? Caption = default,
    [property: JsonPropertyName("CaptionSrc")] string? CaptionSrc = default,
    [property: JsonPropertyName("Description")] string? Description = default,
    [property: JsonPropertyName("DescriptionSrc")] string? DescriptionSrc = default,
    [property: JsonPropertyName("OriginalName")] string? OriginalName = default,
    [property: JsonPropertyName("Name")] string? Name = default,
    [property: JsonPropertyName("Path")] string? Path = default,
    [property: JsonPropertyName("Type")] string? Type = default,
    [property: JsonPropertyName("TypeSrc")] string? TypeSrc = default,
    [property: JsonPropertyName("TakenAt")] DateTime? TakenAt = default,
    [property: JsonPropertyName("TakenAtLocal")] DateTime? TakenAtLocal = default,
    [property: JsonPropertyName("TakenSrc")] string? TakenSrc = default,
    [property: JsonPropertyName("TimeZone")] string? TimeZone = default,
    [property: JsonPropertyName("Year")] int Year = default,
    [property: JsonPropertyName("Month")] int Month = default,
    [property: JsonPropertyName("Day")] int Day = default,
    [property: JsonPropertyName("Lat")] double? Lat = default,
    [property: JsonPropertyName("Lng")] double? Lng = default,
    [property: JsonPropertyName("Altitude")] int Altitude = default,
    [property: JsonPropertyName("Country")] string? Country = default,
    [property: JsonPropertyName("PlaceID")] string? PlaceID = default,
    [property: JsonPropertyName("PlaceSrc")] string? PlaceSrc = default,
    [property: JsonPropertyName("CellID")] string? CellID = default,
    [property: JsonPropertyName("CellAccuracy")] int CellAccuracy = default,
    [property: JsonPropertyName("CameraID")] int CameraID = default,
    [property: JsonPropertyName("CameraSerial")] string? CameraSerial = default,
    [property: JsonPropertyName("CameraSrc")] string? CameraSrc = default,
    [property: JsonPropertyName("LensID")] int LensID = default,
    [property: JsonPropertyName("FocalLength")] int FocalLength = default,
    [property: JsonPropertyName("FNumber")] float FNumber = default,
    [property: JsonPropertyName("Iso")] int Iso = default,
    [property: JsonPropertyName("Exposure")] string? Exposure = default,
    [property: JsonPropertyName("Quality")] int Quality = default,
    [property: JsonPropertyName("Resolution")] int Resolution = default,
    [property: JsonPropertyName("Color")] int Color = default,
    [property: JsonPropertyName("Duration")] TimeSpan? Duration = default,
    [property: JsonPropertyName("Faces")] int Faces = default,
    [property: JsonPropertyName("Favorite")] bool Favorite = default,
    [property: JsonPropertyName("Private")] bool Private = default,
    [property: JsonPropertyName("Scan")] bool Scan = default,
    [property: JsonPropertyName("Panorama")] bool Panorama = default,
    [property: JsonPropertyName("Stack")] int Stack = default,
    [property: JsonPropertyName("DocumentID")] string? DocumentID = default,
    [property: JsonPropertyName("EstimatedAt")] DateTime? EstimatedAt = default,
    [property: JsonPropertyName("createdAt")] DateTime? CreatedAt = default,
    [property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt = default,
    [property: JsonPropertyName("deletedAt")] DateTime? DeletedAt = default,
    [property: JsonPropertyName("editedAt")] DateTime? EditedAt = default,
    [property: JsonPropertyName("checkedAt")] DateTime? CheckedAt = default,
    [property: JsonPropertyName("PublishedAt")] DateTime? PublishedAt = default,
    [property: JsonPropertyName("CreatedBy")] string? CreatedBy = default,
    [property: JsonPropertyName("Details")] PhotoDetails? Details = default,
    [property: JsonPropertyName("files")] IReadOnlyList<File>? Files = default,
    [property: JsonPropertyName("labels")] IReadOnlyList<PhotoLabel>? Labels = default,
    [property: JsonPropertyName("Albums")] IReadOnlyList<Album>? Albums = default
) : IPhoto
{
    IPhotoDetails? IPhoto.Details => Details;
    IReadOnlyList<IFile>? IPhoto.Files => Files?.Cast<IFile>().ToList();
    IReadOnlyList<IPhotoLabel>? IPhoto.Labels => Labels?.Cast<IPhotoLabel>().ToList();
    IReadOnlyList<IAlbum>? IPhoto.Albums => Albums?.Cast<IAlbum>().ToList();
}
