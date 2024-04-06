using ClickStitch.Api.Patterns.CreatePattern;
using ClickStitch.Api.Patterns.CreatePattern.Types;
using Core.Types;
using DotNetLibs.Core.Types;
using Microsoft.AspNetCore.Http;

namespace Integration.Tests.Fakes;

public sealed class FakeCreatePatternService : ICreatePatternService
{
    public Task<Result> CreatePattern(
        RequestUser requestUser,
        CreatePatternRequest request,
        string patternData,
        IFormFile thumbnail,
        IFormFile? bannerImage,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}