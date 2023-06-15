namespace ClickStitch.Api.Users.Types;

public sealed class GetSelfResponse
{
    public required UserModel User { get; init; }
    public required List<PermissionModel> Permissions { get; init; }
}