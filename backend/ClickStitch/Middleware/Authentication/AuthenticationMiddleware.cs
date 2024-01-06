using ClickStitch.Api.Auth;
using ClickStitch.Helper;
using Data.Repositories.User;
using DotNetLibs.Api.Types;
using System.Net;

namespace ClickStitch.Middleware.Authentication;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AuthenticateAttribute : Attribute;

public sealed class AuthenticationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var attribute = context.GetEndpoint()?.Metadata.GetMetadata<AuthenticateAttribute>();
        if (attribute == null)
        {
            await next(context);
            return;
        }

        var cancellationToken = context.RequestAborted;

        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            await next(context);
            return;
        }

        if (!authHeader.ToString().StartsWith("Bearer "))
        {
            await Unauthorized(context);
            return;
        }

        var bearer = authHeader.ToString().Split("Bearer ")[1];

        var loginTokenService = context.RequestServices.GetRequiredService<ILoginTokenService>();

        var userReferenceResult = loginTokenService.GetUserReferenceByToken(bearer);
        if (userReferenceResult.IsFailure)
        {
            await Unauthorized(context);
            return;
        }

        var userRepository = context.RequestServices.GetRequiredService<IUserRepository>();

        var userResult = await userRepository.GetWithPermissionsByReferenceAsync(userReferenceResult.Content, cancellationToken);
        if (!userResult.TrySuccess(out var user))
        {
            await Unauthorized(context);
            return;
        }

        context.Items[RequestHelper.REQUEST_USER_ITEM_KEY] = new RequestUser
        {
            Id = user.Id,
            Reference = user.Reference,
            Permissions = user.Permissions.Select(x => (RequestPermissionType)x.Type).ToList()
        };

        await next(context);
    }

    private static async Task Unauthorized(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsJsonAsync(new ApiErrorResponse("Unable to authenticate user."));
    }
}