﻿using CrossStitchViewer.Api.Auth.Attributes;
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
}