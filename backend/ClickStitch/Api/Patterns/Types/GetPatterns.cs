namespace ClickStitch.Api.Patterns.Types;

public sealed class SearchPatternsResponse
{
    public required List<PatternModel> Patterns { get; init; }
}