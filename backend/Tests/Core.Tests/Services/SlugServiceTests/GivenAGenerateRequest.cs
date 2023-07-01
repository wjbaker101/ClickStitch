using Core.Services;

namespace Core.Tests.Services.SlugServiceTests;

[TestFixture("Test Pattern TITLE", "test-pattern-title")]
[TestFixture("Test Pattern Title!!!", "test-pattern-title")]
[TestFixture("test_pattern_title", "testpatterntitle")]
[TestFixture("test----pattern----title", "test-pattern-title")]
[TestFixture("!!test--123!----title!", "test-123-title")]
[Parallelizable]
public sealed class GivenAGenerateRequest
{
    private readonly string _incomingText;
    private readonly string _expectedSlug;

    private string _result = null!;

    public GivenAGenerateRequest(string incomingText, string expectedSlug)
    {
        _incomingText = incomingText;
        _expectedSlug = expectedSlug;
    }

    [OneTimeSetUp]
    public void Setup()
    {
        _result = SlugService.Generate(_incomingText);
    }

    [Test]
    public void ThenTheCorrectResultIsReturned()
    {
        Assert.That(_result, Is.EqualTo(_expectedSlug));
    }
}