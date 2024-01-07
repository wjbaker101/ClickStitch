using ClickStitch.Helper;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace ClickStitch.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireAdminAttribute : Attribute, IAsyncAuthorizationFilter, IOrderedFilter
{
    public int Order => 101;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = RequestHelper.GetRequiredUser(context.HttpContext.Request);

        if (user.Permissions.All(x => x != RequestPermissionType.Admin))
        {
            context.Result = new ContentResult
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Content = JsonConvert.SerializeObject(new ApiErrorResponse("Unable to access endpoint, you are not logged in as an admin."))
            };
            return;
        }
    }
}