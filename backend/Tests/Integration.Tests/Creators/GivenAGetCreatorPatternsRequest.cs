using ClickStitch.Api.Creators.GetCreatorBySelf.Types;
using NUnit.Framework;

namespace Integration.Tests.Creators;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorPatternsRequest : IntegrationTest
{
    private GetCreatorBySelfResponse _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        AsCreator();

        _result = await DoRequest<GetCreatorBySelfResponse>(HttpMethod.Get, "api/creators/self");
    }

    [Test]
    public void ThenTheCorrectCreatorIsReturned()
    {
        var creator = _result.Creator;

        Assert.That(creator?.Reference, Is.EqualTo(Guid.Parse("8e13a53d-67b8-4329-ae3c-dfa64fe69dbf")));
    }
}