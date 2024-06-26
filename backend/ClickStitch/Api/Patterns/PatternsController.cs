﻿using ClickStitch.Api.Patterns.CreatePattern;
using ClickStitch.Api.Patterns.CreatePattern.Types;
using ClickStitch.Api.Patterns.DeletePattern;
using ClickStitch.Api.Patterns.GetPattern;
using ClickStitch.Api.Patterns.GetPatternInventory;
using ClickStitch.Api.Patterns.SearchPatterns;
using ClickStitch.Api.Patterns.UpdatePattern;
using ClickStitch.Api.Patterns.UpdatePattern.Types;
using ClickStitch.Api.Patterns.VerifyPattern;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClickStitch.Api.Patterns;

[Route("api/patterns")]
public sealed class PatternsController : ApiController
{
    private readonly IDeletePatternService _deletePatternService;
    private readonly ICreatePatternService _createPatternService;
    private readonly IGetPatternService _getPatternService;
    private readonly IGetPatternInventoryService _getPatternInventoryService;
    private readonly ISearchPatternsService _searchPatternsService;
    private readonly IUpdatePatternService _updatePatternService;
    private readonly IVerifyPatternService _verifyPatternService;

    public PatternsController(
        IDeletePatternService deletePatternService,
        ICreatePatternService createPatternService,
        IGetPatternService getPatternService,
        IGetPatternInventoryService getPatternInventoryService,
        ISearchPatternsService searchPatternsService,
        IUpdatePatternService updatePatternService,
        IVerifyPatternService verifyPatternService)
    {
        _deletePatternService = deletePatternService;
        _createPatternService = createPatternService;
        _getPatternService = getPatternService;
        _getPatternInventoryService = getPatternInventoryService;
        _searchPatternsService = searchPatternsService;
        _updatePatternService = updatePatternService;
        _verifyPatternService = verifyPatternService;
    }

    [HttpDelete]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> DeletePattern([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _deletePatternService.DeletePattern(user, patternReference, cancellationToken);

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

        var result = await _createPatternService.CreatePattern(requestUser, request, patternDataAsString, thumbnail, bannerImage, cancellationToken);

        return ToApiResponse(result);
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
    [Route("{patternReference:guid}/inventory")]
    [Authenticate]
    public async Task<IActionResult> GetPatternInventory([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getPatternInventoryService.GetPatternInventory(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("")]
    [Authenticate]
    public async Task<IActionResult> SearchPatterns(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetOptionalUser(Request);

        var result = await _searchPatternsService.SearchPatterns(user, cancellationToken);
        
        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> UpdatePattern([FromRoute] Guid patternReference, [FromBody] UpdatePatternRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _updatePatternService.UpdatePattern(user, patternReference, request, cancellationToken);
        
        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("verify")]
    [Authenticate]
    public IActionResult VerifyPattern([FromForm(Name = "pattern_data")] string patternDataAsString, CancellationToken cancellationToken)
    {
        var requestUser = RequestHelper.GetRequiredUser(Request);

        var result = _verifyPatternService.VerifyPattern(patternDataAsString, cancellationToken);
        
        return ToApiResponse(result);
    }
}