using Api.Tests.Inventory.SearchThreads._Helper;
using ClickStitch.Api.Inventory.SearchInventoryThreads;
using ClickStitch.Api.Inventory.SearchInventoryThreads.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Inventory.SearchThreads;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchThreadsRequest
{
    private Result<SearchInventoryThreadsResponse> _result = null!;

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

        var database = new TestDatabase
        {
            Records = SearchThreadsHelper.SetupThreads(user, new List<IDatabaseRecord>
            {
                user
            })
        };

        var parameters = new SearchInventoryThreadsParameters
        {
            SearchTerm = null,
            Brand = null
        };

        var subject = new SearchInventoryThreadsService(null!, new UserRepository(database), new UserThreadRepository(database));

        _result = await subject.SearchInventoryThreads(new TestRequestUser(), parameters, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectNumberOfInventoryThreadsAreReturned()
    {
        Assert.That(_result.Content.InventoryThreads.Count, Is.EqualTo(2));
    }

    [Test]
    public void ThenTheCorrectNumberOfAvailableThreadsAreReturned()
    {
        Assert.That(_result.Content.AvailableThreads.Count, Is.EqualTo(0));
    }

    [Test]
    public void ThenTheCorrectInventoryThreadsAreReturned()
    {
        Assert.Multiple(() =>
        {
            var inventoryThread1 = _result.Content.InventoryThreads[0];

            Assert.That(inventoryThread1.Thread.Reference, Is.EqualTo(Guid.Parse("4d03c4c2-4858-4ad0-91a4-91a67c54376b")));
            Assert.That(inventoryThread1.Count, Is.EqualTo(7224));

            var inventoryThread2 = _result.Content.InventoryThreads[1];

            Assert.That(inventoryThread2.Thread.Reference, Is.EqualTo(Guid.Parse("1a3eda3e-2533-4c47-8ca5-94fbe471fa48")));
            Assert.That(inventoryThread2.Count, Is.EqualTo(623));
        });
    }

    [Test]
    public void ThenTheCorrectAvailableThreadsAreReturned()
    {
        Assert.Multiple(() =>
        {
        });
    }
}