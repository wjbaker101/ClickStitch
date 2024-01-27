using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPermission;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Admin.RemovePermissionFromUser;

[TestFixture]
[Parallelizable]
public sealed class GivenARemovePermissionFromUserRequestForAUserWithoutThePermission
{
    private readonly Guid _userReference = Guid.Parse("94b34b62-0d72-4014-b614-57d131ea3023");

    private Result<RemovePermissionFromUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Reference = _userReference,
            CreatedAt = default,
            Email = null!,
            Password = null!,
            PasswordSalt = null!,
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>()
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user
            }
        };

        var subject = new AdminService(null!, new UserRepository(database), null!, new UserPermissionRepository(database));

        _result = await subject.RemovePermissionFromUser(_userReference, ApiPermissionType.Admin, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("Unable to remove permission as the user does not have it."));
    }
}