using ClickStitch.Api.Admin.AssignPermissionToUser;
using ClickStitch.Api.Admin.AssignPermissionToUser.Types;
using ClickStitch.Api.Admin.GetPermissions;
using ClickStitch.Api.Admin.RemovePermissionFromUser;
using ClickStitch.Api.Admin.SearchUsers;
using ClickStitch.Middleware.Authentication;
using ClickStitch.Middleware.Authorisation;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Admin;

[Route("api/admin")]
public sealed class AdminController : ApiController
{
    private readonly IAssignPermissionToUserService _assignPermissionToUserService;
    private readonly IGetPermissionsService _getPermissionsService;
    private readonly IRemovePermissionFromUserService _removePermissionFromUserService;
    private readonly ISearchUsersService _searchUsersService;

    public AdminController(
        IAssignPermissionToUserService assignPermissionToUserService,
        IGetPermissionsService getPermissionsService,
        IRemovePermissionFromUserService removePermissionFromUserService,
        ISearchUsersService searchUsersService)
    {
        _assignPermissionToUserService = assignPermissionToUserService;
        _getPermissionsService = getPermissionsService;
        _removePermissionFromUserService = removePermissionFromUserService;
        _searchUsersService = searchUsersService;
    }

    [HttpPost]
    [Route("users/{userReference:guid}/permissions")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> AssignPermissionToUser([FromRoute] Guid userReference, [FromBody] AssignPermissionToUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _assignPermissionToUserService.AssignPermissionToUser(userReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("permissions")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var result = await _getPermissionsService.GetPermissions(cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("users")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> SearchUsers([FromQuery(Name = "page_number")] int pageNumber, [FromQuery(Name = "page_size")] int pageSize, CancellationToken cancellationToken)
    {
        var result = await _searchUsersService.SearchUsers(pageNumber, pageSize, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("users/{userReference:guid}/permissions/{permissionType:int}")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> RemovePermissionFromUser([FromRoute] Guid userReference, [FromRoute] int permissionType, CancellationToken cancellationToken)
    {
        var result = await _removePermissionFromUserService.RemovePermissionFromUser(userReference, (ApiPermissionType)permissionType, cancellationToken);

        return ToApiResponse(result);
    }
}