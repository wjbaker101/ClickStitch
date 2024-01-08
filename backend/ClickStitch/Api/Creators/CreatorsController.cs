using ClickStitch.Api.Creators.Types;
using ClickStitch.Middleware.Authentication;
using ClickStitch.Middleware.Authorisation;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Creators;

[Route("api/creators")]
public sealed class CreatorsController : ApiController
{
    private readonly ICreatorsService _creatorsService;

    public CreatorsController(ICreatorsService creatorsService)
    {
        _creatorsService = creatorsService;
    }

    [HttpPost]
    [Route("")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> CreateCreator([FromBody] CreateCreatorRequest request, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _creatorsService.CreateCreator(requestUser, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{creatorReference:guid}")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> UpdateCreator([FromRoute] Guid creatorReference, [FromBody] UpdateCreatorRequest request, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _creatorsService.UpdateCreator(requestUser, creatorReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{creatorReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> GetCreator([FromRoute] Guid creatorReference, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _creatorsService.GetCreator(requestUser, creatorReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("self")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> GetByUser(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _creatorsService.GetCreatorByUser(user, cancellationToken);

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

        var result = await _creatorsService.GetCreatorPatterns(requestUser, creatorReference, pageSize, pageNumber, cancellationToken);

        return ToApiResponse(result);
    }
}