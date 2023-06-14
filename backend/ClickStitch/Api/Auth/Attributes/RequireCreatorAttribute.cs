using ClickStitch.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClickStitch.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireCreatorAttribute : Attribute, IAsyncAuthorizationFilter, IOrderedFilter
{
    public int Order => 101;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = RequestHelper.GetRequiredUser(context.HttpContext.Request);

        if (user.Permissions.All(x => x != RequestPermissionType.Creator))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}