using ClickStitch.Api.Patterns.UpdatePattern;
using ClickStitch.Api.Patterns.UpdatePattern.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeUpdatePatternService : IUpdatePatternService
{
    public Task<Result<UpdatePatternResponse>> UpdatePattern(RequestUser requestUser, Guid patternReference, UpdatePatternRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}