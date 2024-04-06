using ClickStitch.Api.Patterns.Types;
using ClickStitch.Api.Patterns.UpdatePattern;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Patterns.UpdatePattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdatePatternRequestAsAStitcher
{
    private readonly Guid _patternReference = Guid.Parse("97d9d9df-d3e3-42a5-b03d-3a6fdd35df22");

    private TestDatabase _database = null!;

    private Result<UpdatePatternResponse> _result = null!;

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

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new PatternRecord
                {
                    Reference = _patternReference,
                    CreatedAt = default,
                    Title = "TestTitle",
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
                    Threads = null!
                }
            }
        };

        var request = new UpdatePatternRequest
        {
            Title = "NewTestTitle",
            ExternalShopUrl = "NewTestExternalShopUrl",
            AidaCount = 7942
        };

        var subject = new UpdatePatternService(new PatternRepository(_database), new UserRepository(_database));

        _result = await subject.UpdatePattern(new TestRequestUser(), _patternReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenThePatternIsUpdatedCorrectly()
    {
        var pattern = _database.Actions.Updated.OfType<PatternRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(pattern.Title, Is.EqualTo("NewTestTitle"), nameof(pattern.Title));
            Assert.That(pattern.ExternalShopUrl, Is.Null, nameof(pattern.ExternalShopUrl));
            Assert.That(pattern.AidaCount, Is.EqualTo(7942), nameof(pattern.AidaCount));
        });
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        var pattern = _result.Content.Pattern;

        Assert.Multiple(() =>
        {
            Assert.That(pattern.Reference, Is.EqualTo(_patternReference), nameof(pattern.Reference));
            Assert.That(pattern.Title, Is.EqualTo("NewTestTitle"), nameof(pattern.Title));
            Assert.That(pattern.ExternalShopUrl, Is.Null, nameof(pattern.ExternalShopUrl));
            Assert.That(pattern.AidaCount, Is.EqualTo(7942), nameof(pattern.AidaCount));
        });
    }
}