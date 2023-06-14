namespace ClickStitch.Api.Admin.Types;

public sealed class AssignPermissionToUserRequest
{
    public required ApiPermissionType PermissionType { get; init; }
}

public sealed class AssignPermissionToUserResponse
{
}