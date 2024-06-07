using ClickStitch.Api.Creators.UpdateCreator;
using ClickStitch.Api.Creators.UpdateCreator.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Creators.UpdateCreator;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateCreatorRequest
{
    private readonly Guid _creatorReference = Guid.Parse("59f06f0c-a301-467f-abea-55e12a9035ae");

    private TestDatabase _database = null!;

    private Result<UpdateCreatorResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Id = TestRequestUser.USER_ID,
            Reference = default,
            CreatedAt = default,
            Email = null!,
            Password = null!,
            PasswordSalt = null!,
            LastLoginAt = null,
            Permissions = null!
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new CreatorRecord
                {
                    Reference = _creatorReference,
                    CreatedAt = default,
                    Name = "TestName",
                    StoreUrl = "TestStoreUrl",
                    Description = "TestDescription",
                    Users = new List<UserRecord>
                    {
                        user
                    },
                    Patterns = null!
                }
            }
        };

        var request = new UpdateCreatorRequest
        {
            Name = "NewTestName",
            StoreUrl = "NewTestStoreUrl"
        };

        var subject = new UpdateCreatorService(new CreatorRepository(_database), new UserRepository(_database));

        _result = await subject.UpdateCreator(new TestRequestUser(), _creatorReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCreatorIsUpdatedCorrectly()
    {
        var creator = _database.Actions.Updated.OfType<CreatorRecord>().Single();

        Assert.That(creator.Name, Is.EqualTo("NewTestName"));
        Assert.That(creator.StoreUrl, Is.EqualTo("NewTestStoreUrl"));
    }
}