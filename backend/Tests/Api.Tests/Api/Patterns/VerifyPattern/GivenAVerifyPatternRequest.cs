using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Types;

namespace Api.Tests.Api.Patterns.VerifyPattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAVerifyPatternRequest
{
    private Result<VerifyPatternResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var patternParserService = new FakePatternParserService();

        var subject = new PatternsService(null!, null!, null!, null!, null!, null!, null!, patternParserService);

        _result = await subject.VerifyPattern(null!, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }
}