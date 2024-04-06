namespace ClickStitch.Api.Patterns.UpdatePattern.Types;

public sealed class UpdatePatternRequest
{
    public required string Title { get; init; }
    public string? ExternalShopUrl { get; init; }
    public required int AidaCount { get; init; }
}

public sealed class UpdatePatternResponse
{
    public required PatternModel Pattern { get; init; }
}