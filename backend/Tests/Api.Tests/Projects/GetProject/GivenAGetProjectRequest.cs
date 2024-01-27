using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
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
            User = null!,
            Creator = null,
            Threads = new HashSet<PatternThreadRecord>()
        };

        var thread = new PatternThreadRecord
        {
            Id = 0,
            Pattern = pattern,
            Name = null!,
            Description = null!,
            Index = 0,
            Colour = null!
        };
        pattern.Threads.Add(thread);

        var stitch = new PatternThreadStitchRecord
        {
            Id = 0,
            Thread = thread,
            X = 0,
            Y = 0,
            LookupHash = null!
        };

        var userStitch = new UserPatternThreadStitchRecord
        {
            Id = 0,
            User = user,
            Stitch = stitch,
            StitchedAt = default
        };

        var project = new UserPatternRecord
        {
            Id = 0,
            User = user,
            Pattern = pattern,
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
                stitch,
                userStitch
            }
        };

        var subject = new ProjectsService(new UserRepository(database), new UserPatternRepository(database), new PatternRepository(database), new UserPatternThreadStitchRepository(database), null!);

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
        Assert.That(_result.Content.Threads, Is.Not.Null);
    }
}