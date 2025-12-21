namespace PhotoPrism.Sdk;

public interface IClientFactoryManager
{

    /// <summary>
    /// Sets the active instance for the factory.
    /// </summary>
    IClient SetActiveInstance(string instanceKey);

}
