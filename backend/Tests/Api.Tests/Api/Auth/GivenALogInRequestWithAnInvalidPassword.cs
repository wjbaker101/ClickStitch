using ClickStitch.Api.Auth;
using ClickStitch.Api.Auth.Types;
using TestHelpers.Settings;

namespace Api.Tests.Api.Auth;

[TestFixture]
[Parallelizable]
public sealed class GivenALogInRequestWithAnInvalidPassword
{
    private Result<LogInResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var subject = new AuthService(FakeUserRepository.Default(), new PasswordService(new TestAppSecrets()), null!, null!);

        _result = await subject.LogIn(new LogInRequest
        {
            Email = "",
            Password = "InvalidPassword"
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("The given password was incorrect, please try again."));
    }
}