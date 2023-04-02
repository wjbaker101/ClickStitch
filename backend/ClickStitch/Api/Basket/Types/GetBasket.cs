using ClickStitch.Models;

namespace ClickStitch.Api.Basket.Types;

public sealed class GetBasketResponse
{
    public required BasketModel Basket { get; init; }
}