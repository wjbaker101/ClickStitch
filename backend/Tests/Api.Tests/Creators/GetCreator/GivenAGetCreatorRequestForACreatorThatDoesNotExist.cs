using ClickStitch.Api.Creators.GetCreator;
using ClickStitch.Api.Creators.GetCreator.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Creators.GetCreator;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorRequestForACreatorThatDoesNotExist
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
                    Reference = Guid.Parse("c993b99b-b4a9-4d3c-9a62-53688ed7b8d6"),
                    CreatedAt = new DateTime(2021, 06, 13, 13, 59, 33),
                    Name = "TestName",
                    StoreUrl = "TestStoreUrl",
                    Description = "TestDescription",
                    Users = [],
                    Patterns = []
                }
            }
        };

        var subject = new GetCreatorService(new CreatorRepository(database));

        _result = await subject.GetCreator(Guid.Parse("0d270084-13b1-49ab-abad-66325fc16b73"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectFailureMessage()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("Unable to find creator with reference: '0d270084-13b1-49ab-abad-66325fc16b73'."));
    }
}