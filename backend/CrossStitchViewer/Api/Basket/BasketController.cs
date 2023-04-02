using CrossStitchViewer.Api.Auth.Attributes;
using CrossStitchViewer.Helper;
using CrossStitchViewer.Types;
using Microsoft.AspNetCore.Mvc;

namespace CrossStitchViewer.Api.Basket;

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
    public async Task<IActionResult> GetBasket()
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _basketService.GetBasket(user);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("item/{patternReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> AddToBasket([FromRoute] Guid patternReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _basketService.AddToBasket(user, patternReference);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("item/{patternReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> RemoveFromBasket([FromRoute] Guid patternReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _basketService.RemoveFromBasket(user, patternReference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("complete")]
    [Authorisation]
    public async Task<IActionResult> CompleteBasket()
    {
        var user = RequestHelper.GetUser(Request);

        var result = await _basketService.CompleteBasket(user);

        return ToApiResponse(result);
    }
}