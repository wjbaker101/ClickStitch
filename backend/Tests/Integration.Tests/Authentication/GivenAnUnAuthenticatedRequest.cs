using ClickStitch.Api.Patterns.Types;
using NUnit.Framework;

namespace Integration.Tests.Authentication;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUnAuthenticatedRequest : IntegrationTest
{
    private GetPatternsResponse _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _result = await DoRequest<GetPatternsResponse>(HttpMethod.Get, "api/patterns");
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        Assert.That(_result.Patterns[0].Reference, Is.EqualTo(Guid.Parse("c3245143-c616-4b42-87dc-0ba20c52e4e2")));
    }
}