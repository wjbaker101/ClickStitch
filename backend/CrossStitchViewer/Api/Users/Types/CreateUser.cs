using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Users.Types;

public sealed class CreateUserRequest
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public sealed class CreateUserResponse
{
    public required UserModel User { get; init; }
}