namespace ClickStitch.Api.Patterns.Types;

public sealed class UpdatePatternRequest
{
    public required string Title { get; init; }
    public required string ExternalShopUrl { get; init; }
    public int? AidaCount { get; init; }
}

public sealed class UpdatePatternResponse
{
    public required PatternModel Pattern { get; init; }
}