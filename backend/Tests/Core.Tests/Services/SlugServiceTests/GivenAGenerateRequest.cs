using Core.Services;
using DotNetLibs.Core.Types;

namespace Core.Tests.Services.SlugServiceTests;

[TestFixture("Test Pattern TITLE", "test-pattern-title")]
[TestFixture("Test Pattern Title!!!", "test-pattern-title")]
[TestFixture("test_pattern_title", "testpatterntitle")]
[TestFixture("test----pattern----title", "test-pattern-title")]
[TestFixture("!!test--123!----title!", "test-123-title")]
[TestFixture("Test", "test")]
[TestFixture("123", "123")]
[TestFixture("a!!!", "a")]
[Parallelizable]
public sealed class GivenAGenerateRequest
{
    private readonly string _incomingText;
    private readonly string _expectedSlug;

    private Result<string> _result = null!;

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
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectResultIsReturned()
    {
        Assert.That(_result.Content, Is.EqualTo(_expectedSlug));
    }
}