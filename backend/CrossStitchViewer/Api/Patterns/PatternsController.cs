using CrossStitchViewer.Api.Auth.Attributes;
using CrossStitchViewer.Api.Patterns.Types;
using CrossStitchViewer.Helper;
using CrossStitchViewer.Types;
using Microsoft.AspNetCore.Mvc;

namespace CrossStitchViewer.Api.Patterns;

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
    public async Task<IActionResult> CreatePattern()
    {
        var result = await _patternsService.CreatePattern();
        
        return ToApiResponse(result);
    }

    [HttpPatch]
    [Route("{patternReference:guid}/image")]
    public async Task<IActionResult> UpdatePatternImage([FromRoute] Guid patternReference, UpdatePatternImageRequest request)
    {
        var result = await _patternsService.UpdatePatternImage(patternReference, request);
        
        return ToApiResponse(result);
    }
}