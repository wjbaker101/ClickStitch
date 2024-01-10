using ClickStitch.Api.Inventory;
using ClickStitch.Api.Inventory.Types;
using Data.Records;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Inventory.SearchThreads;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchThreadsRequestWithASearchTerm
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

        var thread1 = new ThreadRecord
        {
            Id = 6319,
            Reference = Guid.Parse("1a3eda3e-2533-4c47-8ca5-94fbe471fa48"),
            Brand = null,
            Code = "TestCode1",
            Colour = null
        };

        var thread2 = new ThreadRecord
        {
            Id = 4531,
            Reference = Guid.Parse("4d03c4c2-4858-4ad0-91a4-91a67c54376b"),
            Brand = null,
            Code = "TestCode2",
            Colour = null
        };

        var userThread = new UserThreadRecord
        {
            User = user,
            Thread = thread1,
            Count = 7224
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                userThread,
                thread1,
                thread2
            }
        };

        var parameters = new SearchThreadsParameters
        {
            SearchTerm = "TestCode"
        };

        var subject = new InventoryService(new ThreadRepository(database), new UserRepository(database), new UserThreadRepository(database));

        _result = await subject.SearchThreads(new TestRequestUser(), parameters, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectInventoryThreadIsReturned()
    {
        var inventoryThread = _result.Content.InventoryThreads[0];

        Assert.That(inventoryThread.Thread.Reference, Is.EqualTo(Guid.Parse("1a3eda3e-2533-4c47-8ca5-94fbe471fa48")));
        Assert.That(inventoryThread.Thread.Code, Is.EqualTo("TestCode1"));
        Assert.That(inventoryThread.Count, Is.EqualTo(7224));
    }

    [Test]
    public void ThenTheCorrectAvailableThreadIsReturned()
    {
        var availableThread = _result.Content.AvailableThreads[0];

        Assert.That(availableThread.Reference, Is.EqualTo(Guid.Parse("4d03c4c2-4858-4ad0-91a4-91a67c54376b")));
        Assert.That(availableThread.Code, Is.EqualTo("TestCode2"));
    }

    [Test]
    public void ThenTheCorrectNumberOfAvailableThreadsAreReturned()
    {
        Assert.That(_result.Content.AvailableThreads.Count, Is.EqualTo(1));
    }
}