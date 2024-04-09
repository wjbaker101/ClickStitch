using ClickStitch.Api.Patterns.DeletePattern;
using ClickStitch.Api.Patterns.DeletePattern.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Patterns.DeletePattern;

[TestFixture]
[Parallelizable]
public sealed class GivenADeletePatternRequestCreatedByAStitcherThatHasBeenAddedAsAProject
{
    private PatternThreadRecord _thread1 = null!;
    private PatternThreadRecord _thread2 = null!;
    private PatternRecord _pattern = null!;

    private TestDatabase _database = null!;

    private Result<DeletePatternResponse> _result = null!;

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

        _thread1 = new PatternThreadRecord
        {
            Id = 0,
            Pattern = null!,
            Name = null!,
            Description = null!,
            Index = 0,
            Colour = null!,
            Stitches = []
        };

        _thread2 = new PatternThreadRecord
        {
            Id = 0,
            Pattern = null!,
            Name = null!,
            Description = null!,
            Index = 0,
            Colour = null!,
            Stitches = []
        };

        _pattern = new PatternRecord
        {
            Reference = Guid.Parse("6f6f9364-d204-4bdb-813f-a950938163b8"),
            CreatedAt = default,
            Title = null!,
            Width = 0,
            Height = 0,
            Price = 0,
            ThumbnailUrl = null!,
            ThreadCount = 0,
            StitchCount = 0,
            AidaCount = 0,
            BannerImageUrl = null!,
            ExternalShopUrl = null!,
            TitleSlug = null!,
            IsPublic = false,
            User = user,
            Creator = null,
            Threads = new HashSet<PatternThreadRecord>
            {
                _thread1,
                _thread2
            }
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                _pattern,
                new UserPatternRecord
                {
                    Id = 0,
                    User = null!,
                    Pattern = _pattern,
                    Reference = default,
                    CreatedAt = default,
                    PausePositionX = null,
                    PausePositionY = null
                }
            }
        };

        var subject = new DeletePatternService(
            new PatternRepository(_database),
            new PatternThreadRepository(_database),
            new UserRepository(_database),
            new UserPatternRepository(_database));

        _result = await subject.DeletePattern(new TestRequestUser(), Guid.Parse("6f6f9364-d204-4bdb-813f-a950938163b8"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectMessageIsReturned()
    {
        Assert.That(_result.Content.Message, Is.EqualTo("No users had this pattern, so it has been permanently deleted."));
    }

    [Test]
    public void ThenTheCorrectThreadsAreDeleted()
    {
        var threads = _database.Actions.Deleted.OfType<PatternThreadRecord>().ToList();
        var thread1 = threads[0];
        var thread2 = threads[1];

        Assert.That(thread1, Is.EqualTo(_thread1));
        Assert.That(thread2, Is.EqualTo(_thread2));
    }

    [Test]
    public void ThenThePatternIsDeleted()
    {
        var pattern = _database.Actions.Deleted.OfType<PatternRecord>().Single();

        Assert.That(pattern, Is.EqualTo(_pattern));
    }
}