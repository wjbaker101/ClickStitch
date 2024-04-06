using ClickStitch.Api.Patterns.SearchPatterns.Types;
using NUnit.Framework;

namespace Integration.Tests.Authentication;

[TestFixture]
[Parallelizable]
public sealed class GivenAnAuthenticatedRequest : IntegrationTest
{
    private SearchPatternsResponse _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        AsStitcher();

        _result = await DoRequest<SearchPatternsResponse>(HttpMethod.Get, "api/patterns");
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        Assert.That(_result.Patterns[0].Reference, Is.EqualTo(Guid.Parse("2544630d-9502-4cd1-a777-86260451d138")));
    }
}