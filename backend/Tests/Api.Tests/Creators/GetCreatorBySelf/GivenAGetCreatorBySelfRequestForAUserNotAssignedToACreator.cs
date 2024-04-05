using ClickStitch.Api.Creators;
using ClickStitch.Api.Creators.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Creators.GetCreatorBySelf;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorBySelfRequestForAUserNotAssignedToACreator
{
    private Result<GetCreatorByUserResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Id = TestRequestUser.USER_ID,
                    Reference = default,
                    CreatedAt = default,
                    Email = null!,
                    Password = null!,
                    PasswordSalt = null!,
                    LastLoginAt = null,
                    Permissions = null!
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
    public void ThenNoCreatorIsReturned()
    {
        Assert.That(_result.Content.Creator, Is.Null);
    }
}