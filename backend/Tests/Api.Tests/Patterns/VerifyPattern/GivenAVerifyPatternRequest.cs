using ClickStitch.Api.Patterns.Types;
using ClickStitch.Api.Patterns.VerifyPattern;

namespace Api.Tests.Patterns.VerifyPattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAVerifyPatternRequest
{
    private Result<VerifyPatternResponse> _result = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        var patternParserService = new FakePatternParserService();

        var subject = new VerifyPatternService(patternParserService);

        _result = subject.VerifyPattern(null!, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }
}