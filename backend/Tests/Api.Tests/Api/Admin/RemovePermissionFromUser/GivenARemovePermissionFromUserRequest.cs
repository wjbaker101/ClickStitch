using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPermission;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Admin.RemovePermissionFromUser;

[TestFixture]
[Parallelizable]
public sealed class GivenARemovePermissionFromUserRequest
{
    private readonly Guid _userReference = Guid.Parse("94b34b62-0d72-4014-b614-57d131ea3023");

    private UserRecord _user = null!;
    private PermissionRecord _permission = null!;

    private TestDatabase _database = null!;

    private Result<RemovePermissionFromUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _permission = new PermissionRecord
        {
            Type = PermissionType.Admin,
            Name = "Admin"
        };

        _user = new UserRecord
        {
            Reference = _userReference,
            CreatedAt = default,
            Email = null!,
            Password = null!,
            PasswordSalt = null!,
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>
            {
                _permission
            }
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                new UserPermissionRecord
                {
                    User = _user,
                    Permission = _permission,
                    CreatedAt = default
                }
            }
        };

        var subject = new AdminService(null!, new UserRepository(_database), null!, new UserPermissionRepository(_database), null!);

        _result = await subject.RemovePermissionFromUser(_userReference, ApiPermissionType.Admin, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenThePermissionIsRemovedFromTheUser()
    {
        var userPermission = _database.Actions.Deleted.OfType<UserPermissionRecord>().Single();

        Assert.That(userPermission.User, Is.EqualTo(_user));
        Assert.That(userPermission.Permission, Is.EqualTo(_permission));
    }
}