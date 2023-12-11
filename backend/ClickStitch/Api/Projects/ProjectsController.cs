using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Api.Projects.Types;
using ClickStitch.Helper;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Projects;

[Route("api/projects")]
public sealed class ProjectsController : ApiController
{
    private readonly IProjectsService _projectsService;

    public ProjectsController(IProjectsService projectsService)
    {
        _projectsService = projectsService;
    }

    [HttpGet]
    [Route("")]
    [Authenticate]
    public async Task<IActionResult> GetProjects(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _projectsService.GetProjects(user, cancellationToken);

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