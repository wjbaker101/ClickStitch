using ClickStitch.Types;
using Microsoft.AspNetCore.Mvc;

namespace ClickStitch.Api.Creators;

[Route("api/creators")]
public sealed class CreatorsController : ApiController
{
    private readonly ICreatorsService _creatorsService;

    public CreatorsController(ICreatorsService creatorsService)
    {
        _creatorsService = creatorsService;
    }
}