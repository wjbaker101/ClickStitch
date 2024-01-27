using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;
using TestHelpers.Settings;

namespace Api.Tests.Users.Update;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateUserRequestForADifferentUser
{
    private readonly Guid _userReference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53");
    private readonly Guid _differentUserReference = Guid.Parse("e2fb8199-8507-48bc-bdaf-fb97c927e9df");

    private TestDatabase _database = null!;

    private Result<UpdateUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Reference = _userReference,
                    CreatedAt = new DateTime(2023, 06, 02, 11, 56, 01),
                    Email = "test@email.com",
                    Password = "TestPassword",
                    PasswordSalt = "TestPasswordSalt",
                    LastLoginAt = null,
                    Permissions = new List<PermissionRecord>()
                }
            }
        };

        var requestUser = new TestRequestUser
        {
            Reference = _differentUserReference
        };

        var subject = new UsersService(
            new UserRepository(_database),
            new PasswordService(new TestAppSecrets()),
            new FakeGuidProvider(),
            new FakeDateTimeProvider());

        _result = await subject.UpdateUser(requestUser, _userReference, new UpdateUserRequest(), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.False);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("Cannot update a different user."));
    }
}