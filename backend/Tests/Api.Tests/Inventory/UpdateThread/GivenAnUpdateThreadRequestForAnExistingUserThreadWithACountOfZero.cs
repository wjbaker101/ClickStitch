﻿using ClickStitch.Api.Inventory.UpdateInventoryThread;
using ClickStitch.Api.Inventory.UpdateInventoryThread.Types;
using Data.Records;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Inventory.UpdateThread;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateThreadRequestForAnExistingUserThreadWithACountOfZero
{
    private readonly Guid _threadReference = Guid.Parse("46e159ae-1307-4777-9e05-df47d0bbf59a");

    private UserRecord _user = null!;
    private ThreadRecord _thread = null!;

    private TestDatabase _database = null!;

    private Result<UpdateInventoryThreadResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _user = new UserRecord
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

        _thread = new ThreadRecord
        {
            Reference = _threadReference,
            Brand = null!,
            Code = null!,
            Colour = null!
        };

        var userThread = new UserThreadRecord
        {
            User = _user,
            Thread = _thread,
            Count = 2732
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                _thread,
                userThread
            }
        };

        var request = new UpdateInventoryThreadRequest
        {
            Count = 0
        };

        var subject = new UpdateInventoryThreadService(new ThreadRepository(_database), new UserRepository(_database), new UserThreadRepository(_database));

        _result = await subject.UpdateInventoryThread(new TestRequestUser(), _threadReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheUserThreadIsDeletedCorrectly()
    {
        var userThread = _database.Actions.Deleted.OfType<UserThreadRecord>().Single();

        Assert.That(userThread.User, Is.EqualTo(_user), nameof(userThread.User));
        Assert.That(userThread.Thread, Is.EqualTo(_thread), nameof(userThread.Thread));
        Assert.That(userThread.Count, Is.EqualTo(2732), nameof(userThread.Count));
    }
}