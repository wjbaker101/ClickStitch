using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Api.Patterns.Types;
using ClickStitch.Helper;
using ClickStitch.Types;
using Data.Records;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = Utf8Json.JsonSerializer;

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
    [Authorisation]
    public async Task<IActionResult> SearchPatterns()
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _patternsService.GetPatterns(user);
        
        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    [Authorisation(requireTypes: new[]
    {
        PermissionType.Admin
    })]
    public async Task<IActionResult> CreatePattern(
        [FromForm(Name = "thumbnail")] IFormFile thumbnail,
        [FromForm(Name = "banner_image")] IFormFile bannerImage,
        [FromForm(Name = "request_body")] string requestAsString,
        [FromForm(Name = "pattern_data")] string patternDataAsString)
    {
        var request = JsonConvert.DeserializeObject<CreatePatternRequest>(requestAsString)!;
        var patternData = JsonSerializer.Deserialize<CreatePatternData>(patternDataAsString);

        var result = await _patternsService.CreatePattern(request, patternData, thumbnail, bannerImage);
        
        return ToApiResponse(result);
    }

    [HttpPatch]
    [Route("{patternReference:guid}/image")]
    [Authorisation(requireTypes: new[]
    {
        PermissionType.Admin
    })]
    public async Task<IActionResult> UpdatePatternImage([FromRoute] Guid patternReference, UpdatePatternImageRequest request)
    {
        var result = await _patternsService.UpdatePatternImage(patternReference, request);
        
        return ToApiResponse(result);
    }
}