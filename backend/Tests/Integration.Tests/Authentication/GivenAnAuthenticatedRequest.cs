using ClickStitch.Api.Patterns.Types;
using NUnit.Framework;
using System.Net.Http.Headers;

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
            RequestUri = new Uri("api/patterns", UriKind.Relative),
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1c2VyUmVmZXJlbmNlIjoiZjFhMzVmZGUtMzg4My00NWZmLTk2YTQtNmY4MWM4ODQ2ZTljIiwibmJmIjoxNzA0NTc3OTgxLCJleHAiOjE3MDUxODI3ODEsImlhdCI6MTcwNDU3Nzk4MX0.rcEE5x3HlVKzG8SV5z-QUNh3PiEPBnDtYHjKFKii1PHlLuP_7LoqUC9sY2HsUTQqefzdxrXNoWPx8h_S1wVaMw")
            }
        });

        _result = await ExpectBody<GetPatternsResponse>(response.Content);
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        Assert.That(_result?.Patterns[0].Reference, Is.EqualTo(Guid.Parse("2544630d-9502-4cd1-a777-86260451d138")));
    }
}