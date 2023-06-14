namespace ClickStitch.Api.Admin.Types;

public sealed class RemovePermissionFromUserRequest
{
    public required ApiPermissionType PermissionType { get; init; }
}

public sealed class RemovePermissionFromUserResponse
{
}