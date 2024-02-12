using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Share;

[Route("share")]
public sealed class ShareController : ApiController
{
    private readonly IShareService _shareService;

    public ShareController(IShareService shareService)
    {
        _shareService = shareService;
    }

    [HttpGet]
    [Route("projects/{projectReference:guid}")]
    public async Task<IActionResult> ShareProject([FromRoute] Guid projectReference, CancellationToken cancellationToken)
    {
        var result = await _shareService.ShareProject(projectReference, cancellationToken);
        if (result.IsFailure)
            return ToApiResponse(result);

        return File(result.Content, "image/jpeg");
    }
}