using ClickStitch.Api.Projects.GetProject;
using ClickStitch.Api.Projects.GetProject.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadBackStitch;
using Data.Repositories.UserPatternThreadStitch;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.GetProject;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetProjectRequest
{
    private Result<GetProjectResponse> _result = null!;

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

        var pattern = new PatternRecord
        {
            Id = 0,
            Reference = Guid.Parse("37a2c8a9-a30e-4e77-8abd-e2df81b62792"),
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
            User = new UserRecord
            {
                Reference = default,
                CreatedAt = default,
                Email = null!,
                Password = null!,
                PasswordSalt = null!,
                LastLoginAt = null,
                Permissions = null!
            },
            Creator = null,
            Threads = new HashSet<PatternThreadRecord>()
        };

        var thread = new PatternThreadRecord
        {
            Id = 0,
            Pattern = pattern,
            Name = null!,
            Description = null!,
            Index = 57,
            Colour = null!,
            Stitches = [
                [ 1691, 553 ]
            ],
            BackStitches = [
                [ 9198, 208, 7185, 922 ]
            ]
        };
        pattern.Threads.Add(thread);

        var userStitch = new UserPatternThreadStitchRecord
        {
            Id = 0,
            User = user,
            Thread = thread,
            X = 1605,
            Y = 5154,
            CompletedAt = new DateTime(2011, 01, 10, 01, 33, 02)
        };

        var userBackStitch = new UserPatternThreadBackStitchRecord
        {
            Id = 0,
            User = user,
            Thread = thread,
            StartX = 4037,
            StartY = 3155,
            EndX = 392,
            EndY = 7021,
            CompletedAt = new DateTime(2004, 06, 01, 15, 53, 40)
        };

        var project = new UserPatternRecord
        {
            Id = 0,
            User = user,
            Pattern = pattern,
            Reference = default,
            CreatedAt = default,
            PausePositionX = null,
            PausePositionY = null
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                project,
                pattern,
                thread,
                userStitch,
                userBackStitch
            }
        };

        var subject = new GetProjectService(new UserRepository(database), new UserPatternRepository(database), new PatternRepository(database), new UserPatternThreadStitchRepository(database), new UserPatternThreadBackStitchRepository(database));

        _result = await subject.GetProject(new TestRequestUser(), Guid.Parse("37a2c8a9-a30e-4e77-8abd-e2df81b62792"), CancellationToken.None);
    }

    [Test]
    public void ThenTheProjectIsReturned()
    {
        Assert.That(_result.Content.Project, Is.Not.Null);
    }

    [Test]
    public void ThenTheThreadsAreReturned()
    {
        var thread = _result.Content.Threads[0];

        Assert.That(thread.Thread.Index, Is.EqualTo(57));
    }

    [Test]
    public void ThenTheStitchesAreReturned()
    {
        var stitch = _result.Content.Threads[0].Stitches[0];

        Assert.Multiple(() =>
        {
            Assert.That(stitch[0], Is.EqualTo(1691), "[0]");
            Assert.That(stitch[1], Is.EqualTo(553), "[1]");
        });
    }

    [Test]
    public void ThenTheBackStitchesAreReturned()
    {
        var stitch = _result.Content.Threads[0].BackStitches[0];

        Assert.Multiple(() =>
        {
            Assert.That(stitch[0], Is.EqualTo(9198), "[0]");
            Assert.That(stitch[1], Is.EqualTo(208), "[1]");
            Assert.That(stitch[2], Is.EqualTo(7185), "[2]");
            Assert.That(stitch[3], Is.EqualTo(922), "[3]");
        });
    }

    [Test]
    public void ThenTheCompletedStitchesAreReturned()
    {
        var stitch = _result.Content.Threads[0].CompletedStitches[0];

        Assert.Multiple(() =>
        {
            Assert.That(stitch[0], Is.EqualTo(1605), "[0]");
            Assert.That(stitch[1], Is.EqualTo(5154), "[1]");
            Assert.That(stitch[2], Is.EqualTo(new DateTime(2011, 01, 10, 01, 33, 02)), "[2]");
        });
    }

    [Test]
    public void ThenTheCompletedBackStitchesAreReturned()
    {
        var stitch = _result.Content.Threads[0].CompletedBackStitches[0];

        Assert.Multiple(() =>
        {
            Assert.That(stitch[0], Is.EqualTo(4037), "[0]");
            Assert.That(stitch[1], Is.EqualTo(3155), "[1]");
            Assert.That(stitch[2], Is.EqualTo(392), "[2]");
            Assert.That(stitch[3], Is.EqualTo(7021), "[3]");
            Assert.That(stitch[4], Is.EqualTo(new DateTime(2004, 06, 01, 15, 53, 40)), "[4]");
        });
    }
}