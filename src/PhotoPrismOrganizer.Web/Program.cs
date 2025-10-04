using PhotoPrismOrganizer.Web.Components;
using PhotoPrismOrganizer.Web.Extensions;

namespace PhotoPrismOrganizer.Web;

public class Program
{

    public static void Main(string[] args) =>
        WebApplication.CreateBuilder(args)
            .ConfigureServices(services => services
                .AddRazorComponents()
                .AddInteractiveServerComponents()
            )
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
