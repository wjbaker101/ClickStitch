using ClickStitch.Api.Inventory.Types;
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
    [Route("threads/search")]
    [Authenticate]
    public async Task<IActionResult> SearchThreads(
        [FromQuery(Name = "search_term")] string? searchTerm,
        [FromQuery(Name = "brand")] string? brand,
        CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var parameters = new SearchThreadsParameters
        {
            SearchTerm = searchTerm,
            Brand = brand
        };

        var result = await _inventoryService.SearchThreads(requestUser, parameters, cancellationToken);

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