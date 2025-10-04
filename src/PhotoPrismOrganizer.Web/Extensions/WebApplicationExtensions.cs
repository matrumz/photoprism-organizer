namespace PhotoPrismOrganizer.Web.Extensions;

public static class WebApplicationExtensions
{

    public static WebApplication Configure(
        this WebApplication app,
        Action<WebApplication> configureApp
    )
    {
        configureApp(app);
        return app;
    }

}
