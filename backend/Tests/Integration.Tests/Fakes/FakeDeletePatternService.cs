using ClickStitch.Api.Patterns.DeletePattern;
using ClickStitch.Api.Patterns.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeDeletePatternService : IDeletePatternService
{
    public Task<Result<DeletePatternResponse>> DeletePattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}