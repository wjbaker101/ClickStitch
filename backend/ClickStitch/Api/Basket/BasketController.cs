using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Helper;
using ClickStitch.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Basket;

[Route("api/basket")]
public sealed class BasketController : ApiController
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public async Task<IActionResult> GetBasket(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _basketService.GetBasket(user, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("item/{patternReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> AddToBasket([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _basketService.AddToBasket(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("item/{patternReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> RemoveFromBasket([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _basketService.RemoveFromBasket(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("item/{patternReference:guid}/quick")]
    [Authorisation]
    public async Task<IActionResult> QuickAdd([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _basketService.QuickAdd(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("complete")]
    [Authorisation]
    public async Task<IActionResult> CompleteBasket(CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _basketService.CompleteBasket(user, cancellationToken);

        return ToApiResponse(result);
    }
}