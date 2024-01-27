using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using Data.Records;
using Data.Repositories.Permission;
using Data.Repositories.User;
using Data.Repositories.UserPermission;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Admin.AssignPermissionToUser;

[TestFixture]
[Parallelizable]
public sealed class GivenAnAssignPermissionToUserRequest
{
    private readonly Guid _userReference = Guid.Parse("49c3a7d6-be5c-4314-9496-d44a7ad30df1");

    private UserRecord _user = null!;
    private PermissionRecord _permission = null!;

    private TestDatabase _database = null!;

    private Result<AssignPermissionToUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _user = new UserRecord
        {
            Reference = _userReference,
            CreatedAt = default,
            Email = null!,
            Password = null!,
            PasswordSalt = null!,
            LastLoginAt = null,
            Permissions = null!
        };

        _permission = new PermissionRecord
        {
            Type = PermissionType.Creator,
            Name = "TestCreator"
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                _permission
            }
        };

        var request = new AssignPermissionToUserRequest
        {
            PermissionType = ApiPermissionType.Creator
        };

        var subject = new AdminService(null!, new UserRepository(_database), new PermissionRepository(_database), new UserPermissionRepository(_database));

        _result = await subject.AssignPermissionToUser(_userReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserPermissionIsSaved()
    {
        var userPermission = _database.Actions.Saved.OfType<UserPermissionRecord>().Single();

        Assert.That(userPermission.User, Is.EqualTo(_user));
        Assert.That(userPermission.Permission, Is.EqualTo(_permission));
    }
}