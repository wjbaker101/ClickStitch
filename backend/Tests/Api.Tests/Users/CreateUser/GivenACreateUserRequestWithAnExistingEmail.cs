using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;
using TestHelpers.Settings;

namespace Api.Tests.Users.CreateUser;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateUserRequestWithAnExistingEmail
{
    private Result<CreateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Id = 0,
                    Reference = default,
                    CreatedAt = default,
                    Email = "test@email.com",
                    Password = null!,
                    PasswordSalt = null!,
                    LastLoginAt = null,
                    Permissions = null!
                }
            }
        };

        var subject = new UsersService(
            new UserRepository(database),
            new PasswordService(new TestAppSecrets()),
            new FakeGuidProvider
            {
                FakeGuid = Guid.Parse("55993eb0-9824-4dbf-a674-1f5a09205287")
            },
            new FakeDateTimeProvider());

        _result = await subject.CreateUser(new CreateUserRequest
        {
            Email = "test@email.com",
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
        Assert.That(_result.FailureMessage, Is.EqualTo("Cannot use that email, an existing user already has it. Please try again with a different email."));
    }
}