using ClickStitch.Api.Admin.Types;
using ClickStitch.Api.Auth.Attributes;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Admin;

[Route("api/admin")]
public sealed class AdminController : ApiController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    [Route("permissions")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var result = await _adminService.GetPermissions(cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("users")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> SearchUsers([FromQuery(Name = "page_number")] int pageNumber, [FromQuery(Name = "page_size")] int pageSize, CancellationToken cancellationToken)
    {
        var result = await _adminService.SearchUsers(pageNumber, pageSize, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("users/{userReference:guid}/permissions")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> AssignPermissionToUser([FromRoute] Guid userReference, [FromBody] AssignPermissionToUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminService.AssignPermissionToUser(userReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("users/{userReference:guid}/permissions/{permissionType:int}")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> RemovePermissionFromUser([FromRoute] Guid userReference, [FromRoute] int permissionType, CancellationToken cancellationToken)
    {
        var result = await _adminService.RemovePermissionFromUser(userReference, (ApiPermissionType)permissionType, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("threads")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> CreateThread([FromBody] CreateThreadRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminService.CreateThread(request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("threads/{threadReference:guid}")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> UpdateThread([FromRoute] Guid threadReference, [FromBody] UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminService.UpdateThread(threadReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("threads/{threadReference:guid}")]
    [Authenticate]
    [RequireAdmin]
    public async Task<IActionResult> DeleteThread([FromRoute] Guid threadReference, CancellationToken cancellationToken)
    {
        var result = await _adminService.DeleteThread(threadReference, cancellationToken);

        return ToApiResponse(result);
    }
}