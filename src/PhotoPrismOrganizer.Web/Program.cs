using PhotoPrismOrganizer.Web.Components;
using PhotoPrismOrganizer.Web.Extensions;

using Serilog;

namespace PhotoPrismOrganizer.Web;

public class Program
{

    public static void Main(string[] args) =>
        WebApplication.CreateBuilder(args)
            .ConfigureServices((services, builder) => services
                // General Services
                .AddAntiforgery()
                .AddSerilog(new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger()
                )
                // Razor Components
                .AddRazorComponents()
                .AddInteractiveServerComponents()
            )
            .ConfigureServices(PhotoPrism.Sdk.Hosting.Startup.ConfigureServices)
            .Build()
            .Configure(app =>
            {
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error", createScopeForErrors: true);
                    app.UseHsts();
                }
                app.UseHttpsRedirection();
                app.UseAntiforgery();
                app.MapStaticAssets();
                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode();
            })
            .Run();

}
