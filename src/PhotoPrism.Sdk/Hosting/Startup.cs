using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PhotoPrism.Sdk.Hosting;

public static class Startup
{

    public static void ConfigureServices(IServiceCollection services, IHostApplicationBuilder builder)
    {
        // Configure options from configuration section
        services.Configure<PhotoPrismSdkOptions>(
            builder.Configuration.GetSection(PhotoPrismSdkOptions.SectionName));

        // Register default HttpClient for dynamic instance creation
        // Individual instances will have BaseAddress set dynamically by ClientFactory
        services.AddHttpClient();

        // Register ClientFactory as singleton implementing both interfaces
        services.AddSingleton<ClientFactory>();
        services.AddSingleton<IClientFactory>(sp => sp.GetRequiredService<ClientFactory>());
        services.AddSingleton<IClientFactoryManager>(sp => sp.GetRequiredService<ClientFactory>());
    }

}
