using ClickStitch.Api.Inventory.UpdateThread;
using ClickStitch.Api.Inventory.UpdateThread.Types;
using Data.Records;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Inventory.UpdateThread;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdateThreadRequest
{
    private readonly Guid _threadReference = Guid.Parse("46e159ae-1307-4777-9e05-df47d0bbf59a");

    private UserRecord _user = null!;
    private ThreadRecord _thread = null!;

    private TestDatabase _database = null!;

    private Result<UpdateThreadResponse> _result = null!;

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

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                _thread
            }
        };

        var request = new UpdateThreadRequest
        {
            Count = 719
        };

        var subject = new UpdateThreadService(new ThreadRepository(_database), new UserRepository(_database), new UserThreadRepository(_database));

        _result = await subject.UpdateThread(new TestRequestUser(), _threadReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheUserThreadIsSavedCorrectly()
    {
        var userThread = _database.Actions.Saved.OfType<UserThreadRecord>().Single();

        Assert.That(userThread.User, Is.EqualTo(_user), nameof(userThread.User));
        Assert.That(userThread.Thread, Is.EqualTo(_thread), nameof(userThread.Thread));
        Assert.That(userThread.Count, Is.EqualTo(719), nameof(userThread.Count));
    }
}