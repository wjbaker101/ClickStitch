using ClickStitch.Api.Users.CreateUser;
using ClickStitch.Api.Users.CreateUser.Types;
using ClickStitch.Api.Users.DeleteUser;
using ClickStitch.Api.Users.GetSelf;
using ClickStitch.Api.Users.UpdateUser;
using ClickStitch.Api.Users.UpdateUser.Types;
using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Users;

[Route("api/users")]
public sealed class UsersController : ApiController
{
    private readonly ICreateUserService _createUserService;
    private readonly IDeleteUserService _deleteUserService;
    private readonly IGetSelfService _getSelfService;
    private readonly IUpdateUserService _updateUserService;

    public UsersController(ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetSelfService getSelfService, IUpdateUserService updateUserService)
    {
        _createUserService = createUserService;
        _deleteUserService = deleteUserService;
        _getSelfService = getSelfService;
        _updateUserService = updateUserService;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _createUserService.CreateUser(request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("{userReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _deleteUserService.DeleteUser(user, userReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("self")]
    [Authenticate]
    public async Task<IActionResult> GetSelf(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _getSelfService.GetSelf(user, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{userReference:guid}")]
    [Authenticate]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userReference, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _updateUserService.UpdateUser(user, userReference, request, cancellationToken);

        return ToApiResponse(result);
    }
}