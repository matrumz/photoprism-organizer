using Serilog;

using PhotoPrismOrganizer.Common.Sdks.Compatibility;

namespace PhotoPrism.Sdk.Rest.V1;

internal class PhotosRestClient(
    IHttpClientFactory httpClientFactory,
    ILogger logger
) : RestClient(
    httpClientFactory: httpClientFactory,
    logger: logger
)
{

    [Operation("photos.search", since: "1.0.0")]
    public Task<IReadOnlyList<IPhotoSearchResult>> SearchPhotosAsync(
        int count,
        int offset = 0,
        string? order = null,
        bool? merged = null,
        bool? @public = null,
        int? quality = null,
        string? query = null,
        string? albumUid = null,
        string? path = null,
        bool? video = null,
        CancellationToken cancellationToken = default
    ) => GetAsync<IReadOnlyList<IPhotoSearchResult>, PhotoSearchResult[]>(
        path: "/api/v1/photos",
        queryParams: new
        {
            count,
            offset,
            order,
            merged,
            @public,
            quality,
            q = query,
            s = albumUid,
            path,
            video
        },
        operationContext: OperationContext.Common.PhotoSearch,
        cancellationToken: cancellationToken
    );

}
