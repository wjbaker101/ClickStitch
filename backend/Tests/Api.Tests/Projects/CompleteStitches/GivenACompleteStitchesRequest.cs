using ClickStitch.Api.Projects.CompleteStitches;
using ClickStitch.Api.Projects.CompleteStitches.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadStitch;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.CompleteStitches;

[TestFixture]
[Parallelizable]
public sealed class GivenACompleteStitchesRequest
{
    private UserRecord _user = null!;
    private PatternThreadStitchRecord _stitch1 = null!;
    private PatternThreadStitchRecord _stitch2 = null!;
    private PatternThreadStitchRecord _stitch3 = null!;
    private PatternThreadStitchRecord _stitch4 = null!;

    private TestDatabase _database = null!;

    private Result<CompleteStitchesResponse> _result = null!;

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

        var thread1 = new PatternThreadRecord
        {
            Pattern = new PatternRecord
            {
                Reference = Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"),
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
                User = null!,
                Creator = null,
                Threads = null!
            },
            Name = null!,
            Description = null!,
            Index = 1,
            Colour = null!,
            Stitches = []
        };

        var thread2 = new PatternThreadRecord
        {
            Pattern = new PatternRecord
            {
                Reference = Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"),
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
                User = null!,
                Creator = null,
                Threads = null!
            },
            Name = null!,
            Description = null!,
            Index = 2,
            Colour = null!,
            Stitches = []
        };

        _stitch1 = new PatternThreadStitchRecord
        {
            Thread = thread1,
            X = 1,
            Y = 1,
            LookupHash = "1,1"
        };

        _stitch2 = new PatternThreadStitchRecord
        {
            Thread = thread1,
            X = 2,
            Y = 2,
            LookupHash = "2,2"
        };

        _stitch3 = new PatternThreadStitchRecord
        {
            Thread = thread2,
            X = 3,
            Y = 3,
            LookupHash = "3,3"
        };

        _stitch4 = new PatternThreadStitchRecord
        {
            Thread = thread2,
            X = 4,
            Y = 4,
            LookupHash = "4,4"
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                thread1,
                thread2,
                _stitch1,
                _stitch2,
                _stitch3,
                _stitch4
            }
        };

        var request = new CompleteStitchesRequest
        {
            StitchesByThread = new Dictionary<int, List<CompleteStitchesRequest.Position>>
            {
                [1] = new()
                {
                    new CompleteStitchesRequest.Position { X = 1, Y = 1 },
                    new CompleteStitchesRequest.Position { X = 2, Y = 2 }
                },
                [2] = new()
                {
                    new CompleteStitchesRequest.Position { X = 3, Y = 3 },
                    new CompleteStitchesRequest.Position { X = 4, Y = 4 }
                }
            }
        };

        var subject = new CompleteStitchesService(new UserRepository(_database), new UserPatternThreadStitchRepository(_database));

        _result = await subject.CompleteStitches(new TestRequestUser(), Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"), request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectUserStitchesAreSaved()
    {
        var records = _database.Actions.Saved.OfType<UserPatternThreadStitchRecord>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(records[0].User, Is.EqualTo(_user));
            Assert.That(records[0].X, Is.EqualTo(1));
            Assert.That(records[0].Y, Is.EqualTo(1));

            Assert.That(records[1].User, Is.EqualTo(_user));
            Assert.That(records[1].X, Is.EqualTo(2));
            Assert.That(records[1].Y, Is.EqualTo(2));

            Assert.That(records[2].User, Is.EqualTo(_user));
            Assert.That(records[2].X, Is.EqualTo(3));
            Assert.That(records[2].Y, Is.EqualTo(3));

            Assert.That(records[3].User, Is.EqualTo(_user));
            Assert.That(records[3].X, Is.EqualTo(4));
            Assert.That(records[3].Y, Is.EqualTo(4));
        });
    }
}