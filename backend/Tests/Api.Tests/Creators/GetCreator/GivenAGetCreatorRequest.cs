using ClickStitch.Api.Creators.GetCreator;
using ClickStitch.Api.Creators.GetCreator.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Creators.GetCreator;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorRequest
{
    private Result<GetCreatorResponse> _result;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new CreatorRecord
                {
                    Reference = Guid.Parse("0d270084-13b1-49ab-abad-66325fc16b73"),
                    CreatedAt = new DateTime(2021, 06, 13, 13, 59, 33),
                    Name = "TestName",
                    StoreUrl = "TestStoreUrl",
                    Users = [],
                    Patterns = []
                }
            }
        };

        var subject = new GetCreatorService(new CreatorRepository(database));

        _result = await subject.GetCreator(Guid.Parse("0d270084-13b1-49ab-abad-66325fc16b73"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectCreatorIsReturned()
    {
        var creator = _result.Content.Creator;

        Assert.Multiple(() =>
        {
            Assert.That(creator.Reference, Is.EqualTo(Guid.Parse("0d270084-13b1-49ab-abad-66325fc16b73")), nameof(creator.Reference));
            Assert.That(creator.CreatedAt, Is.EqualTo(new DateTime(2021, 06, 13, 13, 59, 33)), nameof(creator.CreatedAt));
            Assert.That(creator.Name, Is.EqualTo("TestName"), nameof(creator.Name));
            Assert.That(creator.StoreUrl, Is.EqualTo("TestStoreUrl"), nameof(creator.StoreUrl));
        });
    }
}