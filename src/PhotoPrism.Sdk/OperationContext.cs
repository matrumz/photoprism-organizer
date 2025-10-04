namespace PhotoPrism.Sdk;

/// <summary>
/// Provides operation context information for API methods.
/// </summary>
/// <param name="operationName">The name of the operation (e.g., "photo search").</param>
/// <param name="operationAction">The action being performed (e.g., "searching photos").</param>
internal record OperationContext(string OperationName, string OperationAction)
{

    /// <summary>
    /// Common operation contexts for PhotoPrism API operations.
    /// </summary>
    public static class Common
    {

        // Photos operations
        public static readonly OperationContext PhotoSearch = new("photo search", "searching photos");
        public static readonly OperationContext PhotoGet = new("photo retrieval", "retrieving photo");
        public static readonly OperationContext PhotoUpdate = new("photo update", "updating photo");
        public static readonly OperationContext PhotoDelete = new("photo deletion", "deleting photo");
        public static readonly OperationContext PhotoLike = new("photo like", "liking photo");
        public static readonly OperationContext PhotoDislike = new("photo dislike", "disliking photo");

        // Albums operations
        public static readonly OperationContext AlbumSearch = new("album search", "searching albums");
        public static readonly OperationContext AlbumGet = new("album retrieval", "retrieving album");
        public static readonly OperationContext AlbumCreate = new("album creation", "creating album");
        public static readonly OperationContext AlbumUpdate = new("album update", "updating album");
        public static readonly OperationContext AlbumDelete = new("album deletion", "deleting album");

        // Labels operations
        public static readonly OperationContext LabelSearch = new("label search", "searching labels");
        public static readonly OperationContext LabelGet = new("label retrieval", "retrieving label");
        public static readonly OperationContext LabelUpdate = new("label update", "updating label");

        // Files operations
        public static readonly OperationContext FileSearch = new("file search", "searching files");
        public static readonly OperationContext FileGet = new("file retrieval", "retrieving file");
        public static readonly OperationContext FileDownload = new("file download", "downloading file");

    }

    /// <summary>
    /// Implicit conversion to string for backwards compatibility.
    /// </summary>
    /// <param name="context">The operation context.</param>
    /// <returns>The operation name.</returns>
    public static implicit operator string(OperationContext context) => context.OperationName;

}
