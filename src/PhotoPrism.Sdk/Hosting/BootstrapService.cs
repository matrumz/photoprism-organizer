using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PhotoPrism.Sdk.Rest.V1;

namespace PhotoPrism.Sdk.Hosting;

internal sealed class BootstrapService(
    ILogger<BootstrapService> logger,
    PhotosRestClient client
) : IHostedService
{

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        var photos = await client.SearchPhotosAsync(count: 100, cancellationToken: cancellationToken);
        logger.LogInformation("Photos found on startup: {Photos}", photos);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
