namespace Data.Repositories.Creator.Types;

public sealed class GetCreatorPatternsParameters
{
    public required int PageSize { get; init; }
    public required int PageNumber { get; init; }
}

public sealed class GetCreatorPatternsDto
{
    public required List<PatternRecord> Patterns { get; init; }
    public required int TotalCount { get; init; }
}