namespace ClickStitch.Api.Admin.RemovePermissionFromUser.Types;

public sealed class RemovePermissionFromUserRequest
{
    public required ApiPermissionType PermissionType { get; init; }
}

public sealed class RemovePermissionFromUserResponse
{
}