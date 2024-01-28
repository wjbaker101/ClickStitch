using ClickStitch.Api.Patterns.Services;
using ClickStitch.Api.Patterns.Types;
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