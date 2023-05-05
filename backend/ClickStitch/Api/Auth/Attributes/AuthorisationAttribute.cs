using ClickStitch.Helper;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPermission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClickStitch.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorisationAttribute : Attribute, IAsyncAuthorizationFilter, IOrderedFilter
{
    public int Order => 100;

    private readonly PermissionType[] _requireTypes;

    public AuthorisationAttribute(PermissionType[]? requireTypes = null)
    {
        _requireTypes = requireTypes ?? Array.Empty<PermissionType>();
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var cancellationToken = context.HttpContext.RequestAborted;

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

        var userResult = await userRepository.GetByReferenceAsync(userReferenceResult.Content, cancellationToken);
        if (!userResult.TrySuccess(out var user))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (_requireTypes.Any())
        {
            var userPermissionRepository = context.HttpContext.RequestServices.GetRequiredService<IUserPermissionRepository>();

            var permissions = await userPermissionRepository.GetByUser(user, cancellationToken);
            var permissionsSet = permissions.Select(x => x.Type).ToHashSet();

            if (_requireTypes.Any(x => !permissionsSet.Contains(x)))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        context.HttpContext.Items[RequestHelper.REQUEST_USER_ITEM_KEY] = new RequestUser
        {
            Id = user.Id,
            Reference = user.Reference
        };
    }
}