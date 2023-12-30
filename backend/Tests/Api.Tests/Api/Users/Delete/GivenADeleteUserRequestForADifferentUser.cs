using ClickStitch.Api.Auth;
using ClickStitch.Api.Users;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;
using TestHelpers.Settings;

namespace Api.Tests.Api.Users.Delete;

[TestFixture]
[Parallelizable]
public sealed class GivenADeleteUserRequestForADifferentUser
{
    private readonly Guid _userReference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53");
    private readonly Guid _differentUserReference = Guid.Parse("eb81cb00-e9d6-47e8-b93a-079ab44ca714");

    private TestDatabase _database = null!;

    private Result<DeleteUserResponse> _result = null!;

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
                    CreatedAt = default,
                    Email = default!,
                    Password = default!,
                    PasswordSalt = default!,
                    LastLoginAt = default,
                    Permissions = default!
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
        
        _result = await subject.DeleteUser(requestUser, _userReference, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.False);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("Cannot delete a different user."));
    }
}