﻿using ClickStitch.Api.Inventory.SearchInventoryThreads;
using ClickStitch.Api.Inventory.SearchInventoryThreads.Types;
using ClickStitch.Api.Inventory.UpdateThread;
using ClickStitch.Api.Inventory.UpdateThread.Types;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Inventory;

[Route("api/inventory")]
public sealed class InventoryController : ApiController
{
    private readonly ISearchInventoryThreadsService _searchInventoryThreadsService;
    private readonly IUpdateThreadService _updateThreadService;

    public InventoryController(ISearchInventoryThreadsService searchInventoryThreadsService, IUpdateThreadService updateThreadService)
    {
        _searchInventoryThreadsService = searchInventoryThreadsService;
        _updateThreadService = updateThreadService;
    }

    [HttpGet]
    [Route("threads/search")]
    [Authenticate]
    public async Task<IActionResult> SearchInventoryThreads(
        [FromQuery(Name = "search_term")] string? searchTerm,
        [FromQuery(Name = "brand")] string? brand,
        CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var parameters = new SearchInventoryThreadsParameters
        {
            SearchTerm = searchTerm,
            Brand = brand
        };

        var result = await _searchInventoryThreadsService.SearchInventoryThreads(requestUser, parameters, cancellationToken);

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