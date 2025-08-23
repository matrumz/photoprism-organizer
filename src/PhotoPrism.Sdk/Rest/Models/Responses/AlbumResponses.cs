namespace PhotoPrism.Sdk.Rest.Models.Responses;

/// <summary>
/// PhotoPrism album search result.
/// </summary>
public class AlbumSearchResponse
{
    public int ID { get; set; }
    public string? UID { get; set; }
    public string? ParentUID { get; set; }
    public string? Slug { get; set; }
    public string? Path { get; set; }
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? Caption { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Filter { get; set; }
    public string? Order { get; set; }
    public string? Template { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Location { get; set; }
    public string? Category { get; set; }
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
