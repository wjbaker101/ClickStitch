using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using TestHelpers.Settings;

namespace Api.Tests.Api.Users.CreateUser;

public static class ExamplePasswords
{
    public const string TOO_SHORT = "Pwd1!";
    public const string TOO_LONG = "Pwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwd1!";
    public const string NO_NUMBER = "TestPassword!";
    public const string NO_UPPERCASE = "testpassword1!";
    public const string NO_LOWERCASE = "TESTPASSWORD1!";
    public const string NO_SYMBOL = "TestPassword1";
}

[TestFixture(ExamplePasswords.TOO_SHORT, "Password length does not meet requirements")]
[TestFixture(ExamplePasswords.TOO_LONG, "Password length does not meet requirements")]
[TestFixture(ExamplePasswords.NO_NUMBER, "Password does not contain a number")]
[TestFixture(ExamplePasswords.NO_UPPERCASE, "Password does not contain an uppercase character")]
[TestFixture(ExamplePasswords.NO_LOWERCASE, "Password does not contain a lowercase character")]
[TestFixture(ExamplePasswords.NO_SYMBOL, "Password does not contain a symbol")]
[Parallelizable]
public sealed class GivenACreateUserRequestWithAnInvalidPassword
{
    private readonly string _password;
    private readonly string _expectedError;

    private Result<CreateUserResponse> _result = null!;

    public GivenACreateUserRequestWithAnInvalidPassword(string password, string expectedError)
    {
        _password = password;
        _expectedError = expectedError;
    }

    [OneTimeSetUp]
    public async Task Setup()
    {
        var subject = new UsersService(null!, new PasswordService(new TestAppSecrets()), null!, null!, null!, null!, null!);

        _result = await subject.CreateUser(new CreateUserRequest
        {
            Email = "test@email.com",
            Password = _password
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
        Assert.That(_result.FailureMessage, Is.EqualTo($"{_expectedError}. Requirements: Password must be between 8 and 60 characters; contain at least 1 number, uppercase character, lowercase character; and a symbol ( !@#$%^&*()_+=\\[{{\\]}};:<>|./?,- )."));
    }
}