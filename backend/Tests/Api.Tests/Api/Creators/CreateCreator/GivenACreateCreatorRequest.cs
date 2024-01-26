using ClickStitch.Api.Creators;
using ClickStitch.Api.Creators.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Repositories.UserCreator;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Creators.CreateCreator;

[TestFixture]
[Parallelizable]
public sealed class GivenACreateCreatorRequest
{
    private readonly Guid _userReference = Guid.Parse("59f06f0c-a301-467f-abea-55e12a9035ae");

    private TestDatabase _database = null!;

    private Result<CreateCreatorResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Id = TestRequestUser.USER_ID,
                    Reference = _userReference,
                    CreatedAt = default,
                    Email = null!,
                    Password = null!,
                    PasswordSalt = null!,
                    LastLoginAt = null,
                    Permissions = null!
                }
            }
        };

        var request = new CreateCreatorRequest
        {
            Name = "TestName",
            StoreUrl = "TestStoreUrl"
        };

        var subject = new CreatorsService(new CreatorRepository(_database), new UserRepository(_database), new UserCreatorRepository(_database));

        _result = await subject.CreateCreator(new TestRequestUser(), request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCreatorIsSavedCorrectly()
    {
        var creator = _database.Actions.Saved.OfType<CreatorRecord>().Single();

        Assert.That(creator.Name, Is.EqualTo("TestName"));
        Assert.That(creator.StoreUrl, Is.EqualTo("TestStoreUrl"));
    }

    [Test]
    public void ThenTheUserIsAssignedToTheCreator()
    {
        var creator = _database.Actions.Saved.OfType<CreatorRecord>().Single();
        var userCreator = _database.Actions.Saved.OfType<UserCreatorRecord>().Single();

        Assert.That(userCreator.User.Reference, Is.EqualTo(_userReference));
        Assert.That(userCreator.Creator, Is.EqualTo(creator));
    }
}