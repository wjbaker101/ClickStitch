using ClickStitch.Api.Patterns.VerifyPattern;
using ClickStitch.Api.Patterns.VerifyPattern.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeVerifyPatternService : IVerifyPatternService
{
    public Result<VerifyPatternResponse> VerifyPattern(string patternData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}