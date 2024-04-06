using ClickStitch.Api.Creators.CreateCreator;
using ClickStitch.Api.Creators.CreateCreator.Types;
using ClickStitch.Api.Creators.GetCreatorBySelf;
using ClickStitch.Api.Creators.GetCreatorPatterns;
using ClickStitch.Api.Creators.UpdateCreator;
using ClickStitch.Api.Creators.UpdateCreator.Types;
using ClickStitch.Middleware.Authentication;
using ClickStitch.Middleware.Authorisation;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Creators;

[Route("api/creators")]
public sealed class CreatorsController : ApiController
{
    private readonly ICreateCreatorService _createCreatorService;
    private readonly IGetCreatorBySelfService _getCreatorBySelfService;
    private readonly IGetCreatorPatternsService _getCreatorPatternsService;
    private readonly IUpdateCreatorService _updateCreatorService;

    public CreatorsController(
        ICreateCreatorService createCreatorService,
        IGetCreatorBySelfService getCreatorBySelfService,
        IGetCreatorPatternsService getCreatorPatternsService,
        IUpdateCreatorService updateCreatorService)
    {
        _createCreatorService = createCreatorService;
        _getCreatorBySelfService = getCreatorBySelfService;
        _getCreatorPatternsService = getCreatorPatternsService;
        _updateCreatorService = updateCreatorService;
    }

    [HttpPost]
    [Route("")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> CreateCreator([FromBody] CreateCreatorRequest request, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _createCreatorService.CreateCreator(requestUser, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("self")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> GetCreatorBySelf(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getCreatorBySelfService.GetCreatorBySelf(user, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{creatorReference:guid}/patterns")]
    [Authenticate]
    public async Task<IActionResult> GetCreatorPatterns(
        [FromRoute] Guid creatorReference,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "page_number")] int pageNumber,
        CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _getCreatorPatternsService.GetCreatorPatterns(requestUser, creatorReference, pageSize, pageNumber, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{creatorReference:guid}")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> UpdateCreator([FromRoute] Guid creatorReference, [FromBody] UpdateCreatorRequest request, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _updateCreatorService.UpdateCreator(requestUser, creatorReference, request, cancellationToken);

        return ToApiResponse(result);
    }
}