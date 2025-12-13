namespace PhotoPrism.Sdk.Hosting;

public sealed partial record PhotoPrismSdkOptions
{

    public Dictionary<string, InstanceOptions> Instances { get; init; } = [];

    public sealed record InstanceOptions
    {
        public required string Url { get; init; }
    }

}
