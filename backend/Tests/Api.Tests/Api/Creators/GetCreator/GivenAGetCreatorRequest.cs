using ClickStitch.Api.Creators;
using ClickStitch.Api.Creators.Types;
using Data.Records;

namespace Api.Tests.Api.Creators.GetCreator;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetCreatorRequest
{
    private Result<GetCreatorResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var creatorRepository = FakeCreatorRepository.WithCreator(x =>
        {
            x.Users.Add(new UserRecord
            {
                Id = 0,
                Reference = Guid.Parse("4429ad1c-cc5a-45ef-ad9c-404aab313324"),
                CreatedAt = new DateTime(2023, 05, 29, 15, 42, 57),
                Email = "test@email.com",
                Password = "",
                PasswordSalt = "",
                LastLoginAt = new DateTime(2023, 05, 29, 15, 43, 36),
                Permissions = new List<PermissionRecord>()
            });

            x.Patterns.Add(new PatternRecord
            {
                Reference = Guid.Parse("80266bcc-3537-4df4-a9fe-f18c2ea3b53c"),
                CreatedAt = new DateTime(2023, 05, 29, 23, 30, 57),
                Title = "TestTitle",
                Width = 6302,
                Height = 8272,
                Price = 17.12m,
                ThumbnailUrl = "TestThumbnailUrl",
                ThreadCount = 4904,
                StitchCount = 1500,
                AidaCount = 8967,
                BannerImageUrl = "TestBannerImageUrl",
                ExternalShopUrl = "TestExternalShopUrl",
                Creator = x,
                TitleSlug = null,
                Stitches = new HashSet<PatternStitchRecord>(),
                Threads = new HashSet<PatternThreadRecord>()
            });
        });


        var subject = new CreatorsService(creatorRepository, null!, null!);

        _result = await subject.GetCreator(new TestRequestUser(), Guid.Parse("3f4a482d-5d96-4e1e-b5ae-7d7f04034fc6"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectCreatorIsReturned()
    {
        var creator = _result.Content.Creator;
        
        Assert.Multiple(() =>
        {
            Assert.That(creator.Reference, Is.Not.EqualTo(default(Guid)), nameof(creator.Reference));
            Assert.That(creator.CreatedAt, Is.Not.EqualTo(default(DateTime)), nameof(creator.CreatedAt));
            Assert.That(creator.Name, Is.EqualTo("TestCreatorName"), nameof(creator.Name));
            Assert.That(creator.StoreUrl, Is.EqualTo("TestStoreUrl"), nameof(creator.StoreUrl));
        });
    }

    [Test]
    public void ThenTheCorrectUserIsReturned()
    {
        var user = _result.Content.Users[0];

        Assert.Multiple(() =>
        {
            Assert.That(user.Reference, Is.Not.EqualTo(default(Guid)), nameof(user.Reference));
            Assert.That(user.CreatedAt, Is.Not.EqualTo(default(DateTime)), nameof(user.CreatedAt));
            Assert.That(user.Email, Is.EqualTo("test@email.com"), nameof(user.Email));
            Assert.That(user.LastLoginAt, Is.EqualTo(new DateTime(2023, 05, 29, 15, 43, 36)), nameof(user.LastLoginAt));
        });
    }

    [Test]
    public void ThenTheCorrectPatternIsReturned()
    {
        var pattern = _result.Content.Patterns[0];

        Assert.Multiple(() =>
        {
            Assert.That(pattern.Reference, Is.Not.EqualTo(default(Guid)), nameof(pattern.Reference));
            Assert.That(pattern.CreatedAt, Is.Not.EqualTo(default(DateTime)), nameof(pattern.CreatedAt));
            Assert.That(pattern.Title, Is.EqualTo("TestTitle"), nameof(pattern.Title));
            Assert.That(pattern.Width, Is.EqualTo(6302), nameof(pattern.Width));
            Assert.That(pattern.Height, Is.EqualTo(8272), nameof(pattern.Height));
            Assert.That(pattern.Price, Is.EqualTo(17.12m), nameof(pattern.Price));
            Assert.That(pattern.ThumbnailUrl, Is.EqualTo("TestThumbnailUrl"), nameof(pattern.ThumbnailUrl));
            Assert.That(pattern.ThreadCount, Is.EqualTo(4904), nameof(pattern.ThreadCount));
            Assert.That(pattern.StitchCount, Is.EqualTo(1500), nameof(pattern.StitchCount));
            Assert.That(pattern.BannerImageUrl, Is.EqualTo("TestBannerImageUrl"), nameof(pattern.BannerImageUrl));
            Assert.That(pattern.ExternalShopUrl, Is.EqualTo("TestExternalShopUrl"), nameof(pattern.ExternalShopUrl));
        });
    }
}