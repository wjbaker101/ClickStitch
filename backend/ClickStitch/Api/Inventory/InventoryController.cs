using ClickStitch.Api.Inventory.Types;
using ClickStitch.Helper;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
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

    [HttpPut]
    [Route("threads/{threadReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> UpdateThread([FromRoute] Guid threadReference, [FromBody] UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _inventoryService.UpdateThread(requestUser, threadReference, request, cancellationToken);

        return ToApiResponse(result);
    }
}