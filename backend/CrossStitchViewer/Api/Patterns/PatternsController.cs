using CrossStitchViewer.Api.Auth.Attributes;
using CrossStitchViewer.Api.Patterns.Types;
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
    public IActionResult SearchPatterns()
    {
        var result = _patternsService.GetPatterns();
        
        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    public IActionResult CreatePattern()
    {
        var result = _patternsService.CreatePattern();
        
        return ToApiResponse(result);
    }

    [HttpPatch]
    [Route("{patternReference:guid}/image")]
    public IActionResult UpdatePatternImage([FromRoute] Guid patternReference, UpdatePatternImageRequest request)
    {
        var result = _patternsService.UpdatePatternImage(patternReference, request);
        
        return ToApiResponse(result);
    }
}