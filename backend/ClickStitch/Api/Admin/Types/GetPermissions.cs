namespace ClickStitch.Api.Admin.Types;

public sealed class GetPermissionsResponse
{
    public required List<PermissionModel> Permissions { get; init; }
}