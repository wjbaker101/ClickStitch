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
}