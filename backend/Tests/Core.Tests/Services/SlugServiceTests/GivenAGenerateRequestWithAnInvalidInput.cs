using ClickStitch.Services;
using DotNetLibs.Core.Types;

namespace Core.Tests.Services.SlugServiceTests;

[TestFixture("!!!", "Generated slug cannot be empty.")]
[Parallelizable]
public sealed class GivenAGenerateRequestWithAnInvalidInput
{
    private readonly string _incomingText;
    private readonly string _expectedError;

    private Result<string> _result = null!;

    public GivenAGenerateRequestWithAnInvalidInput(string incomingText, string expectedError)
    {
        _incomingText = incomingText;
        _expectedError = expectedError;
    }

    [OneTimeSetUp]
    public void Setup()
    {
        _result = SlugService.Generate(_incomingText);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo(_expectedError));
    }
}