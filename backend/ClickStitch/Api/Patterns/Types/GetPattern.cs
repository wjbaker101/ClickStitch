namespace ClickStitch.Api.Patterns.Types;

public sealed class GetPatternResponse
{
    public required PatternModel Pattern { get; init; }
}