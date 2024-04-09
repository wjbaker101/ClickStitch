using ClickStitch.Api.Projects.GetAnalytics;
using ClickStitch.Api.Projects.GetAnalytics.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Repositories.UserPatternThreadStitch;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.GetAnalytics;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetAnalyticsRequest
{
    private readonly Guid _patternReference = Guid.Parse("3b3b472a-c67a-4a5c-95d8-0a0dfa940165");

    private TestDatabase _database = null!;

    private Result<GetAnalyticsResponse> _result = null!;

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

        var thread = new PatternThreadRecord
        {
            Pattern = null!,
            Name = null!,
            Description = null!,
            Index = 0,
            Colour = null!,
            Stitches = []
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
            StitchCount = 4796,
            AidaCount = 0,
            BannerImageUrl = null!,
            ExternalShopUrl = null!,
            TitleSlug = null!,
            IsPublic = false,
            User = null!,
            Creator = null,
            Threads = new HashSet<PatternThreadRecord>
            {
                thread
            }
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
                },
                new UserPatternThreadStitchRecord
                {
                    User = user,
                    Thread = thread,
                    X = 0,
                    Y = 0,
                    CompletedAt = new DateTime(2013, 05, 19, 01, 15, 44)
                }
            }
        };

        var subject = new GetAnalyticsService(new UserRepository(_database), new UserPatternRepository(_database), new PatternRepository(_database), new UserPatternThreadStitchRepository(_database));

        _result = await subject.GetAnalytics(new TestRequestUser(), _patternReference, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheResponseIsMappedCorrectly()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result.Content.TotalStitches, Is.EqualTo(4796));
            Assert.That(_result.Content.CompletedStitches, Is.EqualTo(1));
            Assert.That(_result.Content.RemainingStitches, Is.EqualTo(4795));

            var heading = _result.Content.Data.Headings[0];
            var value = _result.Content.Data.Values[0];

            Assert.That(heading, Is.EqualTo("19/05/2013"));
            Assert.That(value, Is.EqualTo(1));
        });
    }
}