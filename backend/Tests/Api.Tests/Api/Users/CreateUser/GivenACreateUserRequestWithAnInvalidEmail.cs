using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Core.Types;

namespace Api.Tests.Api.Users.CreateUser;

[TestFixture("testEmail.com")]
[TestFixture("test@")]
[TestFixture("email.com")]
[TestFixture("@email.com")]
[TestFixture("test@email")]
[TestFixture("test@email.")]
[Parallelizable]
public sealed class GivenACreateUserRequestWithAnInvalidEmail
{
    private readonly string _email;

    private Result<CreateUserResponse> _result = null!;

    public GivenACreateUserRequestWithAnInvalidEmail(string email)
    {
        _email = email;
    }

    [OneTimeSetUp]
    public async Task Setup()
    {
        var subject = new UsersService(null!, null!, null!, null!);

        _result = await subject.CreateUser(new CreateUserRequest
        {
            Email = _email,
            Password = "TestPassword1!"
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
        Assert.That(_result.FailureMessage, Is.EqualTo("Email is invalid, please try again."));
    }
}