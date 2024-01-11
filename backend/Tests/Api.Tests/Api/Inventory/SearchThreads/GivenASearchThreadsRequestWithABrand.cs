using Api.Tests.Api.Inventory.SearchThreads._Helper;
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
public sealed class GivenASearchThreadsRequestWithABrand
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

        var database = new TestDatabase
        {
            Records = SearchThreadsHelper.SetupThreads(user, new List<IDatabaseRecord>
            {
                user
            })
        };

        var parameters = new SearchThreadsParameters
        {
            SearchTerm = "TestCode",
            Brand = "TestBrand1"
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
    public void ThenTheCorrectNumberOfInventoryThreadsAreReturned()
    {
        Assert.That(_result.Content.InventoryThreads.Count, Is.EqualTo(1));
    }

    [Test]
    public void ThenTheCorrectNumberOfAvailableThreadsAreReturned()
    {
        Assert.That(_result.Content.AvailableThreads.Count, Is.EqualTo(1));
    }

    [Test]
    public void ThenTheCorrectInventoryThreadsAreReturned()
    {
        Assert.Multiple(() =>
        {
            var inventoryThread1 = _result.Content.InventoryThreads[0];

            Assert.That(inventoryThread1.Thread.Reference, Is.EqualTo(Guid.Parse("1a3eda3e-2533-4c47-8ca5-94fbe471fa48")));
            Assert.That(inventoryThread1.Count, Is.EqualTo(623));
        });
    }

    [Test]
    public void ThenTheCorrectAvailableThreadsAreReturned()
    {
        Assert.Multiple(() =>
        {
            var availableThread1 = _result.Content.AvailableThreads[0];

            Assert.That(availableThread1.Reference, Is.EqualTo(Guid.Parse("9e83c942-660a-470f-b6b2-2efcdd41bc51")));
        });
    }
}