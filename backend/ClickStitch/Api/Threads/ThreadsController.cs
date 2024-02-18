using ClickStitch.Api.Threads.Services;
using ClickStitch.Api.Threads.Types;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Threads;

[Route("api/threads")]
public sealed class ThreadsController : ApiController
{
    private readonly IGetThreadsByColourService _getThreadsByColourService;

    public ThreadsController(IGetThreadsByColourService getThreadsByColourService)
    {
        _getThreadsByColourService = getThreadsByColourService;
    }

    [HttpPost]
    [Route("by-colour")]
    public async Task<IActionResult> GetThreadsByColour([FromBody] GetThreadsByColourRequest request, CancellationToken cancellationToken)
    {
        var result = await _getThreadsByColourService.GetThreadsByColour(request, cancellationToken);

        return ToApiResponse(result);
    }
}