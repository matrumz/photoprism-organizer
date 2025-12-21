namespace PhotoPrism.Sdk.Hosting;

public sealed partial record PhotoPrismSdkOptions
{

    public Dictionary<string, InstanceOptions> Instances { get; init; } = [];

    public sealed record InstanceOptions
    {

        /// <summary>
        /// The base URL of the PhotoPrism instance.
        /// </summary>
        /// <remarks>
        /// Must include the scheme (http/https), domain, port (if applicable), and base path (if applicable; typically for proxies).
        /// </remarks>
        public required string Url { get; init; }

        /// <summary>
        /// The deployed PhotoPrism version.
        /// </summary>
        /// <remarks>
        /// Used to determine API compatibility.
        /// </remarks>
        public required string Version { get; init; } = "latest";

    }

}
