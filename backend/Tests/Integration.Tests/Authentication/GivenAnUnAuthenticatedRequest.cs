using ClickStitch.Api.Patterns.Types;
using NUnit.Framework;
using System.Net.Http.Json;

namespace Integration.Tests.Authentication;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUnAuthenticatedRequest : IntegrationTest
{
    private GetPatternsResponse? _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var response = await Client.GetAsync("api/patterns");

        _result = await response.Content.ReadFromJsonAsync<GetPatternsResponse>();
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        Assert.That(_result?.Patterns[0].Reference, Is.EqualTo(Guid.Parse("c3245143-c616-4b42-87dc-0ba20c52e4e2")));
    }
}