﻿using ClickStitch.Api.Projects.AddProject;
using ClickStitch.Api.Projects.CompleteStitches;
using ClickStitch.Api.Projects.GetProject;
using ClickStitch.Api.Projects.GetProjects;
using ClickStitch.Api.Projects.PauseStitching;
using ClickStitch.Api.Projects.Types;
using ClickStitch.Api.Projects.UnCompleteStitches;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Projects;

[Route("api/projects")]
public sealed class ProjectsController : ApiController
{
    private readonly IProjectsService _projectsService;
    private readonly IAddProjectService _addProjectService;
    private readonly ICompleteStitchesService _completeStitchesService;
    private readonly IGetProjectService _getProjectService;
    private readonly IGetProjectsService _getProjectsService;
    private readonly IPauseStitchingService _pauseStitchingService;
    private readonly IUnCompleteStitchesService _unCompleteStitchesService;

    public ProjectsController(
        IProjectsService projectsService,
        IAddProjectService addProjectService,
        ICompleteStitchesService completeStitchesService,
        IGetProjectService getProjectService,
        IGetProjectsService getProjectsService,
        IPauseStitchingService pauseStitchingService,
        IUnCompleteStitchesService unCompleteStitchesService)
    {
        _projectsService = projectsService;
        _getProjectsService = getProjectsService;
        _pauseStitchingService = pauseStitchingService;
        _unCompleteStitchesService = unCompleteStitchesService;
        _addProjectService = addProjectService;
        _completeStitchesService = completeStitchesService;
        _getProjectService = getProjectService;
    }

    [HttpPost]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> AddProject([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _addProjectService.AddProject(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/complete")]
    [Authenticate]
    public async Task<IActionResult> CompleteStitches([FromRoute] Guid patternReference, [FromBody] CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _completeStitchesService.CompleteStitches(user, patternReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> GetProject([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getProjectService.GetProject(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("")]
    [Authenticate]
    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getProjectsService.GetProjects(user, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/pause")]
    [Authenticate]
    public async Task<IActionResult> PauseStitching([FromRoute] Guid patternReference, [FromBody] PauseStitchingRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _pauseStitchingService.PauseStitching(user, patternReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/uncomplete")]
    [Authenticate]
    public async Task<IActionResult> UnCompleteStitches([FromRoute] Guid patternReference, [FromBody] CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _unCompleteStitchesService.UnCompleteStitches(user, patternReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/unpause")]
    [Authenticate]
    public async Task<IActionResult> PauseStitching([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.UnPauseStitching(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{patternReference:guid}/analytics")]
    [Authenticate]
    public async Task<IActionResult> GetAnalytics([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.GetAnalytics(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }
}