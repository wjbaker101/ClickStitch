namespace ClickStitch.Api.Admin.AssignPermissionToUser.Types;

public sealed class AssignPermissionToUserRequest
{
    public required ApiPermissionType PermissionType { get; init; }
}

public sealed class AssignPermissionToUserResponse
{
}