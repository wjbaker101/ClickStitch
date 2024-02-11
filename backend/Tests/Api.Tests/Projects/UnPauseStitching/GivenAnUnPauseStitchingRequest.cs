using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.UnPauseStitching;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUnPauseStitchingRequest
{
    private readonly Guid _patternReference = Guid.Parse("3b3b472a-c67a-4a5c-95d8-0a0dfa940165");

    private TestDatabase _database = null!;

    private Result<UnPauseStitchingResponse> _result = null!;

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
                    Reference = default,
                    CreatedAt = default,
                    PausePositionX = null,
                    PausePositionY = null
                }
            }
        };

        var subject = new ProjectsService(new UserRepository(_database), new UserPatternRepository(_database), new PatternRepository(_database), null!, null!, null!);

        _result = await subject.UnPauseStitching(new TestRequestUser(), _patternReference, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheProjectIsUpdatedWithNoPausePosition()
    {
        var project = _database.Actions.Updated.OfType<UserPatternRecord>().Single();

        Assert.That(project.PausePositionX, Is.Null);
        Assert.That(project.PausePositionY, Is.Null);
    }
}