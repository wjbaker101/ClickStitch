using ClickStitch.Api.Patterns.Services;
using ClickStitch.Api.Patterns.Types;
using ClickStitch.Middleware.Authentication;
using ClickStitch.Middleware.Authorisation;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickStitch.Api.Patterns;

[Route("api/patterns")]
public sealed class PatternsController : ApiController
{
    private readonly IPatternsService _patternsService;
    private readonly IGetPatternInventoryService _getPatternInventoryService;
    private readonly IGetPatternService _getPatternService;

    public PatternsController(IPatternsService patternsService, IGetPatternInventoryService getPatternInventoryService, IGetPatternService getPatternService)
    {
        _patternsService = patternsService;
        _getPatternInventoryService = getPatternInventoryService;
        _getPatternService = getPatternService;
    }

    [HttpGet]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> GetPattern([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getPatternService.GetPattern(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("")]
    [Authenticate]
    public async Task<IActionResult> SearchPatterns(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetOptionalUser(Request);

        var result = await _patternsService.GetPatterns(user, cancellationToken);
        
        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> UpdatePattern([FromRoute] Guid patternReference, [FromBody] UpdatePatternRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _patternsService.UpdatePattern(user, patternReference, request, cancellationToken);
        
        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    [Authenticate]
    public async Task<IActionResult> CreatePattern(
        [FromForm(Name = "thumbnail")] IFormFile thumbnail,
        [FromForm(Name = "banner_image")] IFormFile? bannerImage,
        [FromForm(Name = "request_body")] string requestAsString,
        [FromForm(Name = "pattern_data")] string patternDataAsString,
        CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var request = JsonConvert.DeserializeObject<CreatePatternRequest>(requestAsString)!;

        var result = await _patternsService.CreatePattern(requestUser, request, patternDataAsString, thumbnail, bannerImage, cancellationToken);
        
        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("verify")]
    [Authenticate]
    public IActionResult VerifyPattern([FromForm(Name = "pattern_data")] string patternDataAsString, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = _patternsService.VerifyPattern(patternDataAsString, cancellationToken);
        
        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("{patternReference:guid}")]
    [Authenticate]
    [RequireCreator]
    public async Task<IActionResult> DeletePattern([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _patternsService.DeletePattern(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{patternReference:guid}/inventory")]
    [Authenticate]
    public async Task<IActionResult> GetPatternInventory([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getPatternInventoryService.GetPatternInventory(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }
}