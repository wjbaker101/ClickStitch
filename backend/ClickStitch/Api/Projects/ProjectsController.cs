using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Api.Projects.Types;
using ClickStitch.Helper;
using ClickStitch.Types;
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
    [Authorisation]
    public async Task<IActionResult> GetProjects()
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _projectsService.GetProjects(user);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{patternReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> GetProject([FromRoute] Guid patternReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _projectsService.GetProject(user, patternReference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("{patternReference:guid}/stitches/complete")]
    [Authorisation]
    public async Task<IActionResult> CompleteStitches([FromRoute] Guid patternReference, [FromBody] CompleteStitchesRequest request)
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _projectsService.CompleteStitches(user, patternReference, request);

        return ToApiResponse(result);
    }
}