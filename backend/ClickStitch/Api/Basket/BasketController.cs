using ClickStitch.Api.Auth.Attributes;
using ClickStitch.Helper;
using DotNetLibs.Api.Types;
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

    [HttpPost]
    [Route("item/{patternReference:guid}/quick")]
    [Authenticate]
    public async Task<IActionResult> QuickAdd([FromRoute] Guid patternReference, CancellationToken cancellationToken)
    {
        var user = RequestHelper.GetRequiredUser(Request);

        var result = await _basketService.QuickAdd(user, patternReference, cancellationToken);

        return ToApiResponse(result);
    }
}