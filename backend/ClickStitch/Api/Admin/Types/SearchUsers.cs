namespace ClickStitch.Api.Admin.Types;

public sealed class SearchUsersResponse
{
    public required List<UserModel> Users { get; init; }
    public required PaginationModel Pagination { get; init; }
}