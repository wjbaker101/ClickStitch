using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Projects.PauseStitching;

[TestFixture]
[Parallelizable]
public sealed class GivenAPauseStitchingRequest
{
    private readonly Guid _patternReference = Guid.Parse("3b3b472a-c67a-4a5c-95d8-0a0dfa940165");

    private TestDatabase _database = null!;

    private Result<PauseStitchingResponse> _result = null!;

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
            Reference = _patternReference,
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
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                pattern,
                new UserPatternRecord
                {
                    User = user,
                    Pattern = pattern,
                    CreatedAt = default,
                    PausePositionX = null,
                    PausePositionY = null
                }
            }
        };

        var request = new PauseStitchingRequest
        {
            X = 7583,
            Y = 3948
        };

        var subject = new ProjectsService(new UserRepository(_database), new UserPatternRepository(_database), new PatternRepository(_database), null!, null!);

        _result = await subject.PauseStitching(new TestRequestUser(), _patternReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheProjectIsUpdatedWithTheCorrectPausePosition()
    {
        var project = _database.Actions.Updated.OfType<UserPatternRecord>().Single();

        Assert.That(project.PausePositionX, Is.EqualTo(7583));
        Assert.That(project.PausePositionY, Is.EqualTo(3948));
    }
}