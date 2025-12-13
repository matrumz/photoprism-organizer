using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PhotoPrism.Sdk.Hosting;

public static class Startup
{

    public static void ConfigureServices(IServiceCollection services, IHostApplicationBuilder builder) => services
        .Configure<PhotoPrismSdkOptions>(builder.Configuration.GetSection(PhotoPrismSdkOptions.SectionName))
        .AddSingleton<OptionsPrinter>()
        .AddHttpClient("PhotoPrism", client =>
        {
            var baseUrl = Environment.GetEnvironmentVariable("PHOTOPRISM_SITE_URL") ?? "http://host.docker.internal:2342";
            client.BaseAddress = new Uri(baseUrl);
        })
        ;

}
