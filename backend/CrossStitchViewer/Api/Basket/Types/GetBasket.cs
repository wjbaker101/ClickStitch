using CrossStitchViewer.Models;

namespace CrossStitchViewer.Api.Basket.Types;

public sealed class GetBasketResponse
{
    public required BasketModel Basket { get; init; }
}