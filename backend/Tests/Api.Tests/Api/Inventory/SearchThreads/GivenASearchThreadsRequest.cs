using ClickStitch.Api.Inventory;
using ClickStitch.Api.Inventory.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Inventory.SearchThreads;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchThreadsRequest
{
    private Result<SearchThreadsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Id = TestRequestUser.USER_ID,
            Reference = default,
            CreatedAt = default,
            Email = null,
            Password = null,
            PasswordSalt = null,
            LastLoginAt = null,
            Permissions = null
        };

        var userThread1 = new UserThreadRecord
        {
            User = user,
            Thread = new ThreadRecord
            {
                Reference = Guid.Parse("dab98b21-a3c1-47a6-919b-646b8d7a1086"),
                Brand = null,
                Code = "TestCode_aead1b4c-ba9c-4296-b4be-27e490063db7"
            },
            Count = 6851
        };

        var userThread2 = new UserThreadRecord
        {
            User = user,
            Thread = new ThreadRecord
            {
                Reference = Guid.Parse("c9f52699-e1ba-433d-b1bb-ff45c811604e"),
                Brand = null,
                Code = "TestCode_aead1b4c-ba9c-4296-b4be-27e490063db7"
            },
            Count = 7224
        };

        var thread = new ThreadRecord
        {
            Reference = Guid.Parse("025d6f51-0bb3-4d07-8ccb-df809c5d4320"),
            Brand = null,
            Code = "TestCode_1d826529-f0f0-4949-94dd-4738ee0b17ba"
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                userThread1,
                userThread2,
                thread
            }
        };

        var parameters = new SearchThreadsParameters
        {
            SearchTerm = null
        };

        var subject = new InventoryService(null!, new UserRepository(database), new UserThreadRepository(database));

        _result = await subject.SearchThreads(new TestRequestUser(), parameters, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectInventoryThread1IsReturned()
    {
        var inventoryThread = _result.Content.InventoryThreads[0];

        Assert.That(inventoryThread.Thread.Reference, Is.EqualTo(Guid.Parse("c9f52699-e1ba-433d-b1bb-ff45c811604e")));
        Assert.That(inventoryThread.Count, Is.EqualTo(7224));
    }

    [Test]
    public void ThenTheCorrectInventoryThread2IsReturned()
    {
        var inventoryThread = _result.Content.InventoryThreads[1];

        Assert.That(inventoryThread.Thread.Reference, Is.EqualTo(Guid.Parse("dab98b21-a3c1-47a6-919b-646b8d7a1086")));
        Assert.That(inventoryThread.Count, Is.EqualTo(6851));
    }

    [Test]
    public void ThenNoAvailableThreadsAreReturned()
    {
        Assert.That(_result.Content.AvailableThreads, Is.Empty);
    }
}