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
    public IActionResult GetBasket()
    {
        var user = RequestHelper.GetUser(Request);

        var result = _basketService.GetBasket(user);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("item/{patternReference:guid}")]
    [Authorisation]
    public IActionResult AddToBasket([FromRoute] Guid patternReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = _basketService.AddToBasket(user, patternReference);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("item/{patternReference:guid}")]
    [Authorisation]
    public IActionResult RemoveFromBasket([FromRoute] Guid patternReference)
    {
        var user = RequestHelper.GetUser(Request);

        var result = _basketService.RemoveFromBasket(user, patternReference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("complete")]
    [Authorisation]
    public IActionResult CompleteBasket()
    {
        var user = RequestHelper.GetUser(Request);

        var result = _basketService.CompleteBasket(user);

        return ToApiResponse(result);
    }
}