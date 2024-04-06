namespace ClickStitch.Api.Patterns.CreatePattern.Types;

public sealed class CreatePatternRequest
{
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required int AidaCount { get; init; }
    public required string ExternalShopUrl { get; init; }
}