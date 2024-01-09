namespace ClickStitch.Setup;

public static class SetupFrontend
{
    public static void AddFrontend(this IServiceCollection services)
    {
        services.AddSpaStaticFiles(spa =>
        {
            spa.RootPath = "wwwroot";
        });
    }

    public static void UseFrontend(this WebApplication app)
    {
        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "wwwroot";
        });
    }
}