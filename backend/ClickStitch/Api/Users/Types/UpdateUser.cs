namespace ClickStitch.Api.Users.Types;

public sealed class UpdateUserRequest
{
}

public sealed class UpdateUserResponse
{
    public required UserModel User { get; init; }
}