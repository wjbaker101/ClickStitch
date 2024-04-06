namespace ClickStitch.Api.Patterns.GetPattern.Types;

public sealed class GetPatternResponse
{
    public required PatternModel Pattern { get; init; }
}