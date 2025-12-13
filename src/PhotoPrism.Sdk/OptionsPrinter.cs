using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PhotoPrism.Sdk.Hosting;

namespace PhotoPrism.Sdk;

public sealed class OptionsPrinter(
    IOptionsMonitor<PhotoPrismSdkOptions> photoPrismSdkOptionsMonitor,
    ILogger<OptionsPrinter> logger
)
{
    public void PrintOptions()
    {
        var options = photoPrismSdkOptionsMonitor.CurrentValue;

        logger.LogInformation(JsonSerializer.Serialize(options, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }
}
