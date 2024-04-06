using ClickStitch.Api.Patterns.Types;
using ClickStitch.Api.Patterns.UpdatePattern;
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