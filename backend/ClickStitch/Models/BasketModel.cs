namespace ClickStitch.Models;

public sealed class BasketModel
{
    public required List<BasketItemModel> Items { get; init; }
    public required decimal TotalPrice { get; init; }
}

public sealed class BasketItemModel
{
    public required PatternModel Pattern { get; init; }
    public required DateTime AddedAt { get; init; }
}