using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Api.Patterns.Types;
using ClickStitch.Helper;
using ClickStitch.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickStitch.Api.Patterns;

[Route("api/patterns")]
public sealed class PatternsController : ApiController
{
    private readonly IPatternsService _patternsService;

    public PatternsController(IPatternsService patternsService)
    {
        _patternsService = patternsService;
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
    [RequireCreator]
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
    [RequireCreator]
    public async Task<IActionResult> VerifyPattern([FromForm(Name = "pattern_data")] string patternDataAsString, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = await _patternsService.VerifyPattern(patternDataAsString, cancellationToken);
        
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
}