using ClickStitch.Api.Inventory.SearchThreads;
using ClickStitch.Api.Inventory.SearchThreads.Types;
using ClickStitch.Api.Inventory.UpdateThread;
using ClickStitch.Api.Inventory.UpdateThread.Types;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Inventory;

[Route("api/inventory")]
public sealed class InventoryController : ApiController
{
    private readonly ISearchThreadsService _searchThreadsService;
    private readonly IUpdateThreadService _updateThreadService;

    public InventoryController(ISearchThreadsService searchThreadsService, IUpdateThreadService updateThreadService)
    {
        _searchThreadsService = searchThreadsService;
        _updateThreadService = updateThreadService;
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

        var result = await _searchThreadsService.SearchThreads(requestUser, parameters, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("threads/{threadReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> UpdateThread([FromRoute] Guid threadReference, [FromBody] UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _updateThreadService.UpdateThread(requestUser, threadReference, request, cancellationToken);

        return ToApiResponse(result);
    }
}