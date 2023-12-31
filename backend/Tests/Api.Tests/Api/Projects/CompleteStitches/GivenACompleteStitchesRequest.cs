using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadStitch;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Projects.CompleteStitches;

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
            Reference = Guid.Parse("b75e4944-5f79-4e70-a97b-0f9e92c5d172"),
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
                Title = null,
                Width = 0,
                Height = 0,
                Price = 0,
                ThumbnailUrl = null,
                ThreadCount = 0,
                StitchCount = 0,
                AidaCount = 0,
                BannerImageUrl = null,
                ExternalShopUrl = null,
                TitleSlug = null,
                IsPublic = false,
                User = null,
                Creator = null,
                Threads = null
            },
            Name = null,
            Description = null,
            Index = 1,
            Colour = null
        };

        var thread2 = new PatternThreadRecord
        {
            Pattern = new PatternRecord
            {
                Reference = Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"),
                CreatedAt = default,
                Title = null,
                Width = 0,
                Height = 0,
                Price = 0,
                ThumbnailUrl = null,
                ThreadCount = 0,
                StitchCount = 0,
                AidaCount = 0,
                BannerImageUrl = null,
                ExternalShopUrl = null,
                TitleSlug = null,
                IsPublic = false,
                User = null,
                Creator = null,
                Threads = null
            },
            Name = null,
            Description = null,
            Index = 2,
            Colour = null
        };

        _stitch1 = new PatternThreadStitchRecord
        {
            Thread = thread1,
            X = 0,
            Y = 0,
            LookupHash = "1,1"
        };

        _stitch2 = new PatternThreadStitchRecord
        {
            Thread = thread1,
            X = 0,
            Y = 0,
            LookupHash = "2,2"
        };

        _stitch3 = new PatternThreadStitchRecord
        {
            Thread = thread2,
            X = 0,
            Y = 0,
            LookupHash = "3,3"
        };

        _stitch4 = new PatternThreadStitchRecord
        {
            Thread = thread2,
            X = 0,
            Y = 0,
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

        var requestUser = new TestRequestUser
        {
            Reference = Guid.Parse("b75e4944-5f79-4e70-a97b-0f9e92c5d172")
        };

        var subject = new ProjectsService(new UserRepository(_database), null!, null!, new UserPatternThreadStitchRepository(_database), null!);

        _result = await subject.CompleteStitches(requestUser, Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"), request, CancellationToken.None);
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
            Assert.That(records[0].Stitch, Is.EqualTo(_stitch1));

            Assert.That(records[1].User, Is.EqualTo(_user));
            Assert.That(records[1].Stitch, Is.EqualTo(_stitch2));

            Assert.That(records[2].User, Is.EqualTo(_user));
            Assert.That(records[2].Stitch, Is.EqualTo(_stitch3));

            Assert.That(records[3].User, Is.EqualTo(_user));
            Assert.That(records[3].Stitch, Is.EqualTo(_stitch4));
        });
    }
}