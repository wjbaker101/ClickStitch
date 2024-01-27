using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Types;

namespace Api.Tests.Api.Patterns.VerifyPattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAVerifyPatternRequest
{
    private Result<VerifyPatternResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var patternParserService = new FakePatternParserService();

        var subject = new PatternsService(null!, null!, null!, null!, null!, null!, null!, patternParserService, null!);

        _result = subject.VerifyPattern(null!, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }
}