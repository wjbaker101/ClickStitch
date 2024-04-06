using ClickStitch.Api.Patterns.GetPattern;
using ClickStitch.Api.Patterns.GetPattern.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeGetPatternService : IGetPatternService
{
    public Task<Result<GetPatternResponse>> GetPattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}