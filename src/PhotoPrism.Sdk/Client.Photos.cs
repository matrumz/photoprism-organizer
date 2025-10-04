namespace PhotoPrism.Sdk;

public partial class Client
{

    public async Task<IReadOnlyList<IPhotoSearchResult>> SearchPhotosAsync(
        int count,
        int offset = 0,
        string? order = null,
        bool? merged = null,
        bool? @public = null,
        int? quality = null,
        string? query = null,
        string? albumUid = null,
        string? path = null,
        bool? video = false,
        CancellationToken cancellationToken = default
    )
    {
        var result = await operationRegistry.InvokeAsync("photos.search", new
        {
            count,
            offset,
            order,
            merged,
            @public,
            quality,
            query,
            albumUid,
            path,
            video,
            cancellationToken
        });
        return result switch
        {
            IReadOnlyList<IPhotoSearchResult> photos => photos,
            _ => throw new InvalidOperationException($"Unexpected result type: {result?.GetType().FullName ?? "null"}"),
        };
    }

}
