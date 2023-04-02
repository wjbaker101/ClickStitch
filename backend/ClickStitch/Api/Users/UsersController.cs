using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Api.Users.Types;
using ClickStitch.Helper;
using ClickStitch.Types;
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
    [Authorisation]
    public async Task<IActionResult> GetSelf()
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _usersService.GetSelf(user);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _usersService.CreateUser(request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{userReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userReference, [FromBody] UpdateUserRequest request)
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _usersService.UpdateUser(user, userReference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("{userReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _usersService.DeleteUser(user, userReference);

        return ToApiResponse(result);
    }
}