namespace ClickStitch.Models;

public sealed class PaginationModel
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int PageCount { get; init; }
    public required int TotalCount { get; init; }

    public static PaginationModel Create(int pageNumber, int pageSize, int totalCount) => new()
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        PageCount = (int)Math.Ceiling(totalCount / (double) pageSize),
        TotalCount = totalCount
    };
}