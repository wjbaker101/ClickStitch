using ClickStitch.Api.Admin;
using ClickStitch.Api.Admin.Types;
using ClickStitch.Models;
using Data.Records;
using Data.Repositories.Admin;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Admin.GetPermissions;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetPermissionsRequest
{
    private Result<GetPermissionsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new PermissionRecord
                {
                    Id = 0,
                    Type = PermissionType.Creator,
                    Name = "TestName"
                }
            }
        };

        var subject = new AdminService(new AdminRepository(database), null!, null!, null!);

        _result = await subject.GetPermissions(CancellationToken.None);
    }

    [Test]
    public void ThenTheCorrectPermissionsAreReturned()
    {
        Assert.Multiple(() =>
        {
            var permission = _result.Content.Permissions[0];

            Assert.That(permission.Type, Is.EqualTo(ApiPermissionType.Creator), nameof(permission.Type));
            Assert.That(permission.Name, Is.EqualTo("TestName"), nameof(permission.Name));
        });
    }
}