using ClickStitch.Api.Auth;
using ClickStitch.Api.Auth.Types;
using Core.Types;
using TestHelpers.Fakes;

namespace Api.Tests.Api.Auth;

[TestFixture]
[Parallelizable]
public sealed class GivenALogInRequestThatFailsToGetUser
{
    private Result<LogInResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var userRepository = FakeUserRepository.WithResult(Result.Failure("TestFailureMessage"));

        var subject = new AuthService(userRepository, null!, null!, null!);

        _result = await subject.LogIn(new LogInRequest
        {
            Email = "",
            Password = "TestPassword1!"
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectResponseIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("TestFailureMessage"));
    }
}