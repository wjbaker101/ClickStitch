namespace ClickStitch.Api.Admin.Types;

public sealed class SearchUsersResponse
{
    public required List<UserDetails> Users { get; init; }
    public required PaginationModel Pagination { get; init; }

    public sealed class UserDetails
    {
        public required UserModel User { get; init; }
        public required List<PermissionModel> Permissions { get; init; }
    }
}