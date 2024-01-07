using NUnit.Framework;
using System.Net;

namespace Integration.Tests.Creators;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorPatternsRequestNotAsACreator : IntegrationTest
{
    private (string Response, HttpStatusCode StatusCode) _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        AsStitcher();

        _result = await DoFailureRequest(HttpMethod.Get, "api/creators/self");
    }

    [Test]
    public void ThenTheCorrectStatusCodeIsReturned()
    {
        Assert.That(_result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.Response, Is.EqualTo("Unable to access endpoint, you are not logged in as a creator."));
    }
}