using ClickStitch.Api.Projects.AddProject;
using ClickStitch.Api.Projects.GetProjects;
using ClickStitch.Api.Projects.Types;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Projects;

[Route("api/projects")]
public sealed class ProjectsController : ApiController
{
    private readonly IProjectsService _projectsService;
    private readonly IGetProjectsService _getProjectsService;
    private readonly IAddProjectService _addProjectService;

    public ProjectsController(IProjectsService projectsService, IGetProjectsService getProjectsService, IAddProjectService addProjectService)
    {
        _projectsService = projectsService;
        _getProjectsService = getProjectsService;
        _addProjectService = addProjectService;
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

    [HttpGet]
    [Route("")]
    [Authenticate]
    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getProjectsService.GetProjects(user, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{patternReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> GetProject([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.GetProject(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/complete")]
    [Authenticate]
    public async Task<IActionResult> CompleteStitches([FromRoute] Guid patternReference, [FromBody] CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.CompleteStitches(user, patternReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/uncomplete")]
    [Authenticate]
    public async Task<IActionResult> UnCompleteStitches([FromRoute] Guid patternReference, [FromBody] CompleteStitchesRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.UnCompleteStitches(user, patternReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/pause")]
    [Authenticate]
    public async Task<IActionResult> PauseStitching([FromRoute] Guid patternReference, [FromBody] PauseStitchingRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.PauseStitching(user, patternReference, request, cancellationToken);

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