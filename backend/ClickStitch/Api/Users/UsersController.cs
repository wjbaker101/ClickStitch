using ClickStitch.Api.Users.Types;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Users;

[Route("api/users")]
public sealed class UsersController : ApiController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    [Route("self")]
    [Authenticate]
    public async Task<IActionResult> GetSelf(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _usersService.GetSelf(user, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _usersService.CreateUser(request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{userReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userReference, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _usersService.UpdateUser(user, userReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("{userReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _usersService.DeleteUser(user, userReference, cancellationToken);

        return ToApiResponse(result);
    }
}