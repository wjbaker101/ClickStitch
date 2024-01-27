using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Patterns.GetPatterns;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetPatternsRequestAsAnAnonymousUser
{
    private Result<GetPatternsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var pattern1 = new PatternRecord
        {
            Reference = Guid.Parse("f9d5aca2-cc22-47d1-bea3-265d0c204157"),
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
            IsPublic = true,
            User = null!,
            Creator = null,
            Threads = null!
        };

        var pattern2 = new PatternRecord
        {
            Reference = Guid.Parse("af16dc03-956f-43c5-8aae-6cada5a2e407"),
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
            IsPublic = true,
            User = null!,
            Creator = null,
            Threads = null!
        };

        var privatePattern = new PatternRecord
        {
            Reference = Guid.Parse("27c58681-d0ca-4520-b2f1-59d952e9364c"),
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

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                pattern1,
                pattern2,
                privatePattern
            }
        };

        var subject = new PatternsService(new PatternRepository(database), null!, null!, null!, null!, null!, null!, null!, null!);

        _result = await subject.GetPatterns(null, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenThePrivatePatternIsNotReturned()
    {
        Assert.That(_result.Content.Patterns.Count, Is.EqualTo(2));
        Assert.That(_result.Content.Patterns.Exists(x => x.Reference == Guid.Parse("27c58681-d0ca-4520-b2f1-59d952e9364c")), Is.False);
    }

    [Test]
    public void ThenTheCorrectPatternsAreReturned()
    {
        var pattern1 = _result.Content.Patterns[0];
        var pattern2 = _result.Content.Patterns[1];

        Assert.That(pattern1.Reference, Is.EqualTo(Guid.Parse("f9d5aca2-cc22-47d1-bea3-265d0c204157")));
        Assert.That(pattern2.Reference, Is.EqualTo(Guid.Parse("af16dc03-956f-43c5-8aae-6cada5a2e407")));
    }
}