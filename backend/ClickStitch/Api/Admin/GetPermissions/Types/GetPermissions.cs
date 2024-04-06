namespace ClickStitch.Api.Admin.GetPermissions.Types;

public sealed class GetPermissionsResponse
{
    public required List<PermissionModel> Permissions { get; init; }
}