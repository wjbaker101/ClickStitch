using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Patterns.GetPatterns;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetPatternsRequestAsALoggedInUser
{
    private readonly Guid _userReference = Guid.Parse("f8070524-9fda-44d8-94fc-29d279315b45");

    private Result<GetPatternsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Reference = _userReference,
            CreatedAt = default,
            Email = null,
            Password = null,
            PasswordSalt = null,
            LastLoginAt = null,
            Permissions = null
        };

        var pattern = new PatternRecord
        {
            Reference = Guid.Parse("f9d5aca2-cc22-47d1-bea3-265d0c204157"),
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
            IsPublic = true,
            User = null,
            Creator = null,
            Threads = null
        };

        var existingPattern = new PatternRecord
        {
            Reference = Guid.Parse("af16dc03-956f-43c5-8aae-6cada5a2e407"),
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
            IsPublic = true,
            User = null,
            Creator = null,
            Threads = null
        };

        var privatePattern = new PatternRecord
        {
            Reference = Guid.Parse("27c58681-d0ca-4520-b2f1-59d952e9364c"),
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
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                pattern,
                existingPattern,
                privatePattern,
                new UserPatternRecord
                {
                    User = user,
                    Pattern = existingPattern,
                    CreatedAt = default,
                    PausePositionX = null,
                    PausePositionY = null
                }
            }
        };

        var requestUser = new TestRequestUser
        {
            Reference = _userReference
        };

        var subject = new PatternsService(new PatternRepository(database), null!, null!, new UserRepository(database), new UserPatternRepository(database), null!, null!, null!);

        _result = await subject.GetPatterns(requestUser, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectNumberOfPatternsAreReturned()
    {
        Assert.That(_result.Content.Patterns.Count, Is.EqualTo(1));
    }

    [Test]
    public void ThenThePrivatePatternIsNotReturned()
    {
        Assert.That(_result.Content.Patterns.Exists(x => x.Reference == Guid.Parse("27c58681-d0ca-4520-b2f1-59d952e9364c")), Is.False);
    }

    [Test]
    public void ThenThePatternAlreadyExistingInAProjectIsNotReturned()
    {
        Assert.That(_result.Content.Patterns.Exists(x => x.Reference == Guid.Parse("af16dc03-956f-43c5-8aae-6cada5a2e407")), Is.False);
    }

    [Test]
    public void ThenTheCorrectPatternsAreReturned()
    {
        var pattern = _result.Content.Patterns[0];

        Assert.That(pattern.Reference, Is.EqualTo(Guid.Parse("f9d5aca2-cc22-47d1-bea3-265d0c204157")));
    }
}