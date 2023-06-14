using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Api.Creators.Types;
using ClickStitch.Helper;
using ClickStitch.Types;
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

    [HttpGet]
    [Route("{creatorReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> GetCreator([FromRoute] Guid creatorReference, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _creatorsService.GetCreator(requestUser, creatorReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{creatorReference:guid}/users/{userReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> AssignUserToCreator([FromRoute] Guid creatorReference, [FromRoute] Guid userReference, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _creatorsService.AssignUserToCreator(requestUser, creatorReference, userReference, cancellationToken);

        return ToApiResponse(result);
    }
}