namespace ClickStitch.Api.Admin.Types;

public sealed class GetUsersResponse
{
    public required List<UserModel> Users { get; init; }
    public required PaginationModel Pagination { get; init; }
}