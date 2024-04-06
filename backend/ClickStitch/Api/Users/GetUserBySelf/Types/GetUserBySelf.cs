namespace ClickStitch.Api.Users.GetUserBySelf.Types;

public sealed class GetUserBySelfResponse
{
    public required UserModel User { get; init; }
    public required List<PermissionModel> Permissions { get; init; }
}