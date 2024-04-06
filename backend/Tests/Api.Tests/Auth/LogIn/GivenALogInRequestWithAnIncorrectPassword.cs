using ClickStitch.Api.Auth;
using ClickStitch.Api.Auth.LogIn;
using ClickStitch.Api.Auth.LogIn.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPermission;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;
using TestHelpers.Settings;

namespace Api.Tests.Auth.LogIn;

[TestFixture]
[Parallelizable]
public sealed class GivenALogInRequestWithAnIncorrectPassword
{
    private const string CORRECT_PASSWORD = "TestPassword";
    private const string PASSWORD_SALT = "35b12e44-9a7a-4f6e-9dea-7c6dcde27fc6";

    private TestDatabase _database = null!;

    private Result<LogInResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var passwordService = new PasswordService(new TestAppSecrets());
        var hashedPassword = passwordService.Hash(CORRECT_PASSWORD, PASSWORD_SALT);

        var user = new UserRecord
        {
            Reference = default,
            CreatedAt = default,
            Email = "test@email.com",
            Password = hashedPassword,
            PasswordSalt = PASSWORD_SALT,
            LastLoginAt = null,
            Permissions = null!
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new UserPermissionRecord
                {
                    User = user,
                    Permission = new PermissionRecord
                    {
                        Type = PermissionType.Creator,
                        Name = "Creator"
                    },
                    CreatedAt = default
                }
            }
        };

        var dateTimeProvider = new FakeDateTimeProvider
        {
            FakeUtcNow = new DateTime(3030, 04, 01)
        };

        var request = new LogInRequest
        {
            Email = "Test@email.com",
            Password = "IncorrectPassword"
        };

        var subject = new LogInService(new UserRepository(_database), passwordService, new LoginTokenService(dateTimeProvider, new TestAppSecrets()), new UserPermissionRepository(_database), dateTimeProvider);

        _result = await subject.LogIn(request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorMessageIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("The given password was incorrect, please try again."));
    }
}