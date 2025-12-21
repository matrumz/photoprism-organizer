using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PhotoPrism.Sdk.Hosting;

namespace PhotoPrism.Sdk;

public class ClientFactory(
    ILogger<ClientFactory> logger,
    IOptionsMonitor<PhotoPrismSdkOptions> optionsMonitor
) : IClientFactory, IClientFactoryManager
{

    private readonly Dictionary<string, IClient> _clients = [];

    IClient IClientFactory.CreateClient() => throw new NotImplementedException();

    IClient IClientFactory.CreateClient(string instanceKey) => throw new NotImplementedException();

    IClient IClientFactoryManager.SetActiveInstance(string instanceKey) => throw new NotImplementedException();

}
