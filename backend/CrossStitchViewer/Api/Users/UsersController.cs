using CrossStitchViewer.Api.Auth.Attributes;
using CrossStitchViewer.Api.Users.Types;
using CrossStitchViewer.Helper;
using CrossStitchViewer.Types;
using Microsoft.AspNetCore.Mvc;

namespace CrossStitchViewer.Api.Users;

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
    public IActionResult GetSelf()
    {
        var user = RequestHelper.GetUser(Request);

        var result = _usersService.GetSelf(user);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        var result = _usersService.CreateUser(request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{userReference:guid}")]
    [Authorisation]
    public IActionResult UpdateUser([FromRoute] Guid userReference, [FromBody] UpdateUserRequest request)
    {
        var user = RequestHelper.GetUser(Request);

        var result = _usersService.UpdateUser(user, userReference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("{userReference:guid}")]
    [Authorisation]
    public IActionResult DeleteUser([FromRoute] Guid userReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = _usersService.DeleteUser(user, userReference);

        return ToApiResponse(result);
    }
}