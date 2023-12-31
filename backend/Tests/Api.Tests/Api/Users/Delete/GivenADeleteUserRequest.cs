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
public sealed class GivenADeleteUserRequest
{
    private readonly Guid _userReference = Guid.Parse("85e41403-d6e1-4c50-bf48-50f65713ea53");

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
            Reference = _userReference
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
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserIsDeleted()
    {
        var user = _database.Actions.Deleted.OfType<UserRecord>().Single();

        Assert.That(user.Reference, Is.EqualTo(_userReference));
    }
}