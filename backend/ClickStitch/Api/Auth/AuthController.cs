﻿using ClickStitch.Api.Auth.Types;
using ClickStitch.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Auth;

[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("log_in")]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest request)
    {
        var result = await _authService.LogIn(request);

        return ToApiResponse(result);
    }
}