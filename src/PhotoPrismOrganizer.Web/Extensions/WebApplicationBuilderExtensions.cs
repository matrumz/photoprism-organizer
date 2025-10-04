namespace PhotoPrismOrganizer.Web.Extensions;

public static class WebApplicationBuilderExtensions
{

    public static WebApplicationBuilder ConfigureServices(
        this WebApplicationBuilder builder,
        Action<IServiceCollection> configureServices
    )
    {
        configureServices(builder.Services);
        return builder;
    }

}
