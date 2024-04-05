using ClickStitch.Api.Users.DeleteUser;
using ClickStitch.Api.Users.DeleteUser.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Users.Delete;

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

        var subject = new DeleteUserService(new UserRepository(_database));

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