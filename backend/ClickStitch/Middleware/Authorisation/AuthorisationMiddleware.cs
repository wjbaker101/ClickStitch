using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using System.Net;

namespace ClickStitch.Middleware.Authorisation;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireAdminAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireCreatorAttribute : Attribute;

public sealed class AuthorisationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var user = RequestHelper.GetOptionalUser(context.Request);

        if (context.GetEndpoint()?.Metadata.GetMetadata<RequireAdminAttribute>() != null)
        {
            if (user == null || user.Permissions.All(x => x != RequestPermissionType.Admin))
            {
                await Forbidden(context);
                return;
            }
        }

        if (context.GetEndpoint()?.Metadata.GetMetadata<RequireCreatorAttribute>() != null)
        {
            if (user == null || user.Permissions.All(x => x != RequestPermissionType.Creator))
            {
                await Forbidden(context);
                return;
            }
        }

        await next(context);
    }

    private static async Task Forbidden(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        await context.Response.WriteAsJsonAsync(new ApiErrorResponse("Unable to access endpoint, you are not logged in to a user with the correct permissions."));
    }
}