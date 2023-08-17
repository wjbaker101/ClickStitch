using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Helper;
using ClickStitch.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Inventory;

[Route("api/inventory")]
public sealed class InventoryController : ApiController
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    [Route("threads")]
    [Authenticate]
    public async Task<IActionResult> GetThreads(CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _inventoryService.GetThreads(requestUser, cancellationToken);

        return ToApiResponse(result);
    }
}