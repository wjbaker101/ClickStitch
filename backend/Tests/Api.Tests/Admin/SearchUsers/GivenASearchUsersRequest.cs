using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using Data.Records;
using Data.Repositories.Admin;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Admin.SearchUsers;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchUsersRequest
{
    private const int USER_ID = 5559;

    private Result<SearchUsersResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Id = USER_ID,
            Reference = Guid.Parse("016fc7d5-03c8-4639-a31d-08ba1fd17ed8"),
            CreatedAt = new DateTime(2023, 06, 02, 23, 15, 10),
            Email = "test@email.com",
            Password = "",
            PasswordSalt = "",
            LastLoginAt = new DateTime(2023, 06, 02, 23, 15, 20),
            Permissions = null!
        };

        var database = new TestDatabase
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
                        Name = "TestPermissionName"
                    },
                    CreatedAt = default
                }
            }
        };

        var subject = new AdminService(new AdminRepository(database), null!, null!, null!);

        _result = await subject.SearchUsers(1, 50, CancellationToken.None);
    }

    [Test]
    public void ThenTheUserIsMappedCorrectly()
    {
        var user = _result.Content.Users[0].User;

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.EqualTo(Guid.Parse("016fc7d5-03c8-4639-a31d-08ba1fd17ed8")), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.EqualTo(new DateTime(2023, 06, 02, 23, 15, 10)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.LastLoginAt, Is.EqualTo(new DateTime(2023, 06, 02, 23, 15, 20)), nameof(user.LastLoginAt));
        });
    }

    [Test]
    public void ThenThePermissionIsMappedCorrectly()
    {
        var permission = _result.Content.Users[0].Permissions[0];

        Assert.Multiple(() =>
        {
            Assert.That(permission.Type, Is.EqualTo(ApiPermissionType.Creator), nameof(permission.Type));
            Assert.That(permission.Name, Is.EqualTo("TestPermissionName"), nameof(permission.Name));
        });
    }
}