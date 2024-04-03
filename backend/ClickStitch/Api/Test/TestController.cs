using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Test;

[Route("api/test")]
public sealed class TestController : ApiController
{
    [HttpGet]
    [Route("")]
    [Authenticate]
    public IActionResult ThrowException()
    {
        throw new Exception("Throwing an exception...");
    }
}