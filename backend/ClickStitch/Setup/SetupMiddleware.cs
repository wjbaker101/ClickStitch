using ClickStitch.Middleware.Authentication;
using ClickStitch.Middleware.Authorisation;
using ClickStitch.Middleware.ExceptionHandling;

namespace ClickStitch.Setup;

public static class SetupMiddleware
{
    public static void AddMiddleware(this IServiceCollection services)
    {
        services.AddSingleton<AuthenticationMiddleware>();
        services.AddSingleton<AuthorisationMiddleware>();
        services.AddSingleton<ExceptionHandlingMiddleware>();
    }

    public static void UseMiddleware(this WebApplication app)
    {
        app.UseMiddleware<AuthenticationMiddleware>();
        app.UseMiddleware<AuthorisationMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}