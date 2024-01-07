using ClickStitch.Api.Patterns.Types;
using NUnit.Framework;

namespace Integration.Tests.Authentication;

[TestFixture]
[Parallelizable]
public sealed class GivenAnAuthenticatedRequest : IntegrationTest
{
    private GetPatternsResponse? _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        AsStitcher();

        var response = await Client.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("api/patterns", UriKind.Relative)
        });

        _result = await ExpectBody<GetPatternsResponse>(response.Content);
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        Assert.That(_result?.Patterns[0].Reference, Is.EqualTo(Guid.Parse("2544630d-9502-4cd1-a777-86260451d138")));
    }
}