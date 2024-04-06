namespace ClickStitch.Api.Patterns.SearchPatterns.Types;

public sealed class SearchPatternsResponse
{
    public required List<PatternModel> Patterns { get; init; }
}