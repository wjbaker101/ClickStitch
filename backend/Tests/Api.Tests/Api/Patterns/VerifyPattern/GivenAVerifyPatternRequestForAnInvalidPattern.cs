using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Parsing.Types;
using ClickStitch.Api.Patterns.Types;

namespace Api.Tests.Api.Patterns.VerifyPattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAVerifyPatternRequestForAnInvalidPattern
{
    private Result<VerifyPatternResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var patternParserService = new FakePatternParserService
        {
            Response = Result<ParsePatternResponse>.Failure("TestError")
        };

        var subject = new PatternsService(null!, null!, null!, null!, null!, null!, null!, patternParserService);

        _result = await subject.VerifyPattern(null!, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("TestError"));
    }
}