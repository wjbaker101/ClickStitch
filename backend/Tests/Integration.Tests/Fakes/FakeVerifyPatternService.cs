using ClickStitch.Api.Patterns.Types;
using ClickStitch.Api.Patterns.VerifyPattern;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeVerifyPatternService : IVerifyPatternService
{
    public Result<VerifyPatternResponse> VerifyPattern(string patternData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}