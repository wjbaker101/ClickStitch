using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Types;
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
    [Route("users")]
    [Authorisation]
    [RequireAdmin]
    public async Task<IActionResult> GetUsers([FromQuery(Name = "page_number")] int pageNumber, [FromQuery(Name = "page_size")] int pageSize, CancellationToken cancellationToken)
    {
        var result = await _adminService.GetUsers(pageNumber, pageSize, cancellationToken);

        return ToApiResponse(result);
    }
}