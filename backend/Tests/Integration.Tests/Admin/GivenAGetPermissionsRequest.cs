using ClickStitch.Api.Admin.GetPermissions.Types;
using ClickStitch.Models;
using NUnit.Framework;

namespace Integration.Tests.Admin;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetPermissionsRequest : IntegrationTest
{
    private GetPermissionsResponse _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        AsAdmin();

        _result = await DoRequest<GetPermissionsResponse>(HttpMethod.Get, "api/admin/permissions");
    }

    [Test]
    public void ThenThePermissionIsReturned()
    {
        var permission = _result.Permissions[0];

        Assert.That(permission.Type, Is.EqualTo(ApiPermissionType.Creator));
        Assert.That(permission.Name, Is.EqualTo("Creator"));
    }
}