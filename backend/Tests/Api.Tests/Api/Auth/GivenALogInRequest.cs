using ClickStitch.Api.Auth;
using ClickStitch.Api.Auth.Types;
using ClickStitch.Models;
using Core.Types;
using TestHelpers.Fakes;

namespace Api.Tests.Api.Auth;

[TestFixture]
[Parallelizable]
public sealed class GivenALogInRequest
{
    private Result<LogInResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var dateTime = new DateTime(3052, 11, 29, 14, 58, 13);

        var subject = new AuthService(FakeUserRepository.Default(), new PasswordService(), new LoginTokenService(FakeDateTime.With(dateTime)), FakeUserPermissionRepository.Default());

        _result = await subject.LogIn(new LogInRequest
        {
            Email = "",
            Password = "TestPassword1!"
        }, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectResponseIsReturned()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result.Content.Email, Is.EqualTo("test@email.com"), nameof(_result.Content.Email));
            Assert.That(_result.Content.LoginToken, Does.StartWith("eyJhbGciOi"), nameof(_result.Content.LoginToken));

            var permission = _result.Content.Permissions[0];

            Assert.That(permission.Type, Is.EqualTo(ApiPermissionType.Admin), nameof(permission.Type));
            Assert.That(permission.Name, Is.EqualTo("Admin"), nameof(permission.Name));
        });
    }
}