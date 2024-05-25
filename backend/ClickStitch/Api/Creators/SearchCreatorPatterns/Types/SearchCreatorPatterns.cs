namespace ClickStitch.Api.Creators.SearchCreatorPatterns.Types;

public sealed class SearchCreatorPatternsResponse
{
    public required List<PatternModel> Patterns { get; init; }
    public required List<Guid> ProjectPatternReferencesForUser { get; init; }
    public required PaginationModel Pagination { get; init; }
}