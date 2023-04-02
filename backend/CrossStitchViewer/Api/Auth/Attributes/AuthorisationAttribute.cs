using CrossStitchViewer.Models.Mappers;
using Data.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrossStitchViewer.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorisationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!authHeader.ToString().StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var bearer = authHeader.ToString().Split("Bearer ")[1];

        var loginTokenService = context.HttpContext.RequestServices.GetRequiredService<ILoginTokenService>();

        var userReferenceResult = loginTokenService.GetUserReferenceByToken(bearer);
        if (userReferenceResult.IsFailure)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

        var userResult = await userRepository.GetByReferenceAsync(userReferenceResult.Content);
        if (userResult.IsFailure)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["user"] = UserMapper.Map(userResult.Content);
    }
}