using NUnit.Framework;
using System.Net;

namespace Integration.Tests.Admin;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetPermissionsRequestNotAsAnAdmin : IntegrationTest
{
    private (string Response, HttpStatusCode StatusCode) _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        AsStitcher();

        _result = await DoFailureRequest(HttpMethod.Get, "api/admin/permissions");
    }

    [Test]
    public void ThenTheCorrectStatusCodeIsReturned()
    {
        Assert.That(_result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.Response, Is.EqualTo("Unable to access endpoint, you are not logged in to a user with the correct permissions."));
    }
}