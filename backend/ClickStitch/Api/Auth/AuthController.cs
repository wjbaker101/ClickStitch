using ClickStitch.Api.Auth.LogIn;
using ClickStitch.Api.Auth.LogIn.Types;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Auth;

[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly ILogInService _logInService;

    public AuthController(ILogInService logInService)
    {
        _logInService = logInService;
    }

    [HttpPost]
    [Route("log_in")]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest request, CancellationToken cancellationToken)
    {
        var result = await _logInService.LogIn(request, cancellationToken);

        return ToApiResponse(result);
    }
}