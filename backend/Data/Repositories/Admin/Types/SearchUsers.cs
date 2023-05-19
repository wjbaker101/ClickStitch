namespace Data.Repositories.Admin.Types;

public sealed class SearchUsersParameters
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}

public sealed class SearchUsersDto
{
    public required List<UserRecord> Users { get; init; }
    public required int TotalCount { get; init; }
}