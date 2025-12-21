namespace PhotoPrism.Sdk;

public interface IClientFactory
{

    /// <summary>
    /// Creates a new PhotoPrism SDK client for the active/default instance.
    /// </summary>
    /// <returns></returns>
    IClient CreateClient();

    /// <summary>
    /// Creates a new PhotoPrism SDK client for the specified instance.
    /// </summary>
    /// <param name="instanceKey"></param>
    /// <returns></returns>
    IClient CreateClient(string instanceKey);

}
