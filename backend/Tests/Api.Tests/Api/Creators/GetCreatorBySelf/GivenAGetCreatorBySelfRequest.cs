using ClickStitch.Api.Creators;
using ClickStitch.Api.Users.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Creators.GetCreatorBySelf;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorBySelfRequest
{
    private Result<GetCreatorByUserResponse> _result = null!;

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

        var creator = new CreatorRecord
        {
            Reference = Guid.Parse("61c8e475-ced7-44dc-b016-ebc261a08653"),
            CreatedAt = default,
            Name = null,
            StoreUrl = null,
            Users = null,
            Patterns = null
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new UserCreatorRecord
                {
                    User = user,
                    Creator = creator
                }
            }
        };

        var subject = new CreatorsService(new CreatorRepository(database), new UserRepository(database), null!);

        _result = await subject.GetCreatorBySelf(new TestRequestUser(), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectCreatorIsReturned()
    {
        Assert.That(_result.Content.Creator!.Reference, Is.EqualTo(Guid.Parse("61c8e475-ced7-44dc-b016-ebc261a08653")));
    }
}