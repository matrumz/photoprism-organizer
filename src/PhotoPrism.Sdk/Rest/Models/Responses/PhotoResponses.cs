namespace PhotoPrism.Sdk.Rest.Models.Responses;

/// <summary>
/// PhotoPrism photo search result.
/// </summary>
public class PhotoSearchResponse
{
    public string? UID { get; set; }
    public string? DocumentID { get; set; }
    public string? InstanceID { get; set; }
    public DateTime? TakenAt { get; set; }
    public DateTime? TakenAtLocal { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public DateTime? CheckedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? TakenSrc { get; set; }
    public string? TimeZone { get; set; }
    public string? Path { get; set; }
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public string? Country { get; set; }
    public int Stack { get; set; }
    public bool Favorite { get; set; }
    public bool Private { get; set; }
    public int Iso { get; set; }
    public int FocalLength { get; set; }
    public double FNumber { get; set; }
    public string? Exposure { get; set; }
    public int Quality { get; set; }
    public int Resolution { get; set; }
    public int Color { get; set; }
    public bool Scan { get; set; }
    public bool Panorama { get; set; }
    public int CameraID { get; set; }
    public string? CameraSrc { get; set; }
    public string? CameraMake { get; set; }
    public string? CameraModel { get; set; }
    public string? CameraType { get; set; }
    public string? CameraSerial { get; set; }
    public int LensID { get; set; }
    public string? LensMake { get; set; }
    public string? LensModel { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public int CellAccuracy { get; set; }
    public int Altitude { get; set; }
    public string? PlaceID { get; set; }
    public string? PlaceLabel { get; set; }
    public string? PlaceCity { get; set; }
    public string? PlaceState { get; set; }
    public string? PlaceCountry { get; set; }
    public string? PlaceSrc { get; set; }
    public int Faces { get; set; }
    public string? Hash { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Portrait { get; set; }
    public bool Merged { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Type { get; set; }
    public string? TypeSrc { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? Caption { get; set; }
    public string? FileUID { get; set; }
    public string? FileRoot { get; set; }
    public string? FileName { get; set; }
    public List<FileResponse>? Files { get; set; }
}

/// <summary>
/// PhotoPrism photo details response.
/// </summary>
public class PhotoResponse
{
    public int Id { get; set; }
    public string? UID { get; set; }
    public string? DocumentID { get; set; }
    public DateTime? TakenAt { get; set; }
    public DateTime? TakenAtLocal { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public DateTime? CheckedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? TakenSrc { get; set; }
    public string? TimeZone { get; set; }
    public string? Path { get; set; }
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public string? Title { get; set; }
    public string? TitleSrc { get; set; }
    public string? Description { get; set; }
    public string? DescriptionSrc { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public string? Country { get; set; }
    public int Stack { get; set; }
    public bool Favorite { get; set; }
    public bool Private { get; set; }
    public int Iso { get; set; }
    public int FocalLength { get; set; }
    public double FNumber { get; set; }
    public string? Exposure { get; set; }
    public int Quality { get; set; }
    public int Resolution { get; set; }
    public int Color { get; set; }
    public bool Scan { get; set; }
    public bool Panorama { get; set; }
    public CameraResponse? Camera { get; set; }
    public int CameraID { get; set; }
    public string? CameraSrc { get; set; }
    public string? CameraSerial { get; set; }
    public LensResponse? Lens { get; set; }
    public int LensID { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public CellResponse? Cell { get; set; }
    public int CellAccuracy { get; set; }
    public string? CellID { get; set; }
    public PlaceResponse? Place { get; set; }
    public string? PlaceID { get; set; }
    public string? PlaceSrc { get; set; }
    public int Altitude { get; set; }
    public string? Type { get; set; }
    public string? TypeSrc { get; set; }
    public TimeSpan? Duration { get; set; }
    public int Faces { get; set; }
    public string? Caption { get; set; }
    public string? CaptionSrc { get; set; }
    public DateTime? EstimatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? CreatedBy { get; set; }
    public List<FileResponse>? Files { get; set; }
    public List<PhotoLabelResponse>? Labels { get; set; }
    public List<AlbumResponse>? Albums { get; set; }
    public DetailsResponse? Details { get; set; }
}

/// <summary>
/// PhotoPrism file information.
/// </summary>
public class FileResponse
{
    public string? UID { get; set; }
    public string? Name { get; set; }
    public string? OriginalName { get; set; }
    public string? Hash { get; set; }
    public string? FileType { get; set; }
    public string? MediaType { get; set; }
    public string? Mime { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double AspectRatio { get; set; }
    public bool Portrait { get; set; }
    public bool Primary { get; set; }
    public bool Sidecar { get; set; }
    public bool Missing { get; set; }
    public bool Video { get; set; }
    public TimeSpan? Duration { get; set; }
    public double FPS { get; set; }
    public int Frames { get; set; }
    public long Size { get; set; }
    public string? Codec { get; set; }
    public int MediaUTC { get; set; }
    public DateTime? TakenAt { get; set; }
    public string? TimeIndex { get; set; }
    public int Orientation { get; set; }
    public string? OrientationSrc { get; set; }
    public string? Projection { get; set; }
    public string? ColorProfile { get; set; }
    public string? MainColor { get; set; }
    public string? Colors { get; set; }
    public string? Luminance { get; set; }
    public int Diff { get; set; }
    public int Chroma { get; set; }
    public string? Software { get; set; }
    public bool Watermark { get; set; }
    public bool HDR { get; set; }
    public string? Root { get; set; }
    public int ModTime { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? PhotoUID { get; set; }
    public string? InstanceID { get; set; }
    public int CreatedIn { get; set; }
    public int UpdatedIn { get; set; }
    public string? Error { get; set; }
    public string? MediaID { get; set; }
}

/// <summary>
/// PhotoPrism camera information.
/// </summary>
public class CameraResponse
{
    public int ID { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// PhotoPrism lens information.
/// </summary>
public class LensResponse
{
    public int ID { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// PhotoPrism place information.
/// </summary>
public class PlaceResponse
{
    public string? PlaceID { get; set; }
    public string? Label { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? District { get; set; }
    public string? Keywords { get; set; }
    public bool Favorite { get; set; }
    public int PhotoCount { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// PhotoPrism cell information.
/// </summary>
public class CellResponse
{
    public string? ID { get; set; }
    public string? Name { get; set; }
    public string? Street { get; set; }
    public string? Postcode { get; set; }
    public string? Category { get; set; }
    public PlaceResponse? Place { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// PhotoPrism photo label information.
/// </summary>
public class PhotoLabelResponse
{
    public int LabelID { get; set; }
    public string? LabelSrc { get; set; }
    public int Uncertainty { get; set; }
    public int PhotoID { get; set; }
    public LabelResponse? Label { get; set; }
    public PhotoResponse? Photo { get; set; }
}

/// <summary>
/// PhotoPrism label information.
/// </summary>
public class LabelResponse
{
    public int ID { get; set; }
    public string? UID { get; set; }
    public string? Slug { get; set; }
    public string? CustomSlug { get; set; }
    public string? Name { get; set; }
    public int Priority { get; set; }
    public bool Favorite { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public int PhotoCount { get; set; }
    public string? Thumb { get; set; }
    public string? ThumbSrc { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
}

/// <summary>
/// PhotoPrism album information.
/// </summary>
public class AlbumResponse
{
    public int ID { get; set; }
    public string? UID { get; set; }
    public string? ParentUID { get; set; }
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Path { get; set; }
    public string? Type { get; set; }
    public string? Caption { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Filter { get; set; }
    public string? Order { get; set; }
    public string? Template { get; set; }
    public string? Country { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public bool Favorite { get; set; }
    public bool Private { get; set; }
    public int PhotoCount { get; set; }
    public int LinkCount { get; set; }
    public string? Thumb { get; set; }
    public string? ThumbSrc { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

/// <summary>
/// PhotoPrism photo details.
/// </summary>
public class DetailsResponse
{
    public int PhotoID { get; set; }
    public string? Keywords { get; set; }
    public string? KeywordsSrc { get; set; }
    public string? Notes { get; set; }
    public string? NotesSrc { get; set; }
    public string? Subject { get; set; }
    public string? SubjectSrc { get; set; }
    public string? Artist { get; set; }
    public string? ArtistSrc { get; set; }
    public string? Copyright { get; set; }
    public string? CopyrightSrc { get; set; }
    public string? License { get; set; }
    public string? LicenseSrc { get; set; }
    public string? Software { get; set; }
    public string? SoftwareSrc { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
