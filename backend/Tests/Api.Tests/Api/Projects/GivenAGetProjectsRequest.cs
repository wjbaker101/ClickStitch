using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Projects;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetProjectsRequest
{
    private readonly Guid _userReference = Guid.Parse("28d44e5e-31cf-4827-8d27-bed40409eb9b");

    private Result<GetProjectsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Id = 0,
            Reference = _userReference,
            CreatedAt = default,
            Email = null!,
            Password = null!,
            PasswordSalt = null!,
            LastLoginAt = null,
            Permissions = null!
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new UserPatternRecord
                {
                    User = user,
                    Pattern = new PatternRecord
                    {
                        Reference = Guid.Parse("571099c0-be56-4a49-8dd6-1fb77c95cb04"),
                        CreatedAt = new DateTime(2023, 12, 12, 01, 33, 57),
                        Title = "TestTitle",
                        Width = 8773,
                        Height = 7725,
                        Price = 884.23m,
                        ThumbnailUrl = "TestThumbnailUrl",
                        ThreadCount = 6897,
                        StitchCount = 1446,
                        AidaCount = 243,
                        BannerImageUrl = "TestBannerImageUrl",
                        ExternalShopUrl = "TestExternalShopUrl",
                        TitleSlug = "test-title-slug",
                        IsPublic = true,
                        User = user,
                        Creator = new CreatorRecord
                        {
                            Id = 0,
                            Reference = default,
                            CreatedAt = default,
                            Name = null,
                            StoreUrl = null,
                            Users = null,
                            Patterns = null
                        },
                        Threads = new HashSet<PatternThreadRecord>
                        {
                            new()
                            {
                                Pattern = null!,
                                Name = "TestName",
                                Description = "TestDescription",
                                Index = 9396,
                                Colour = "TestColour"
                            }
                        }
                    },
                    CreatedAt = new DateTime(2023, 12, 07, 11, 19, 00),
                    PausePositionX = null,
                    PausePositionY = null
                }
            }
        };

        var subject = new ProjectsService(new UserRepository(database), new UserPatternRepository(database), null!, null!);

        _result = await subject.GetProjects(new TestRequestUser(), CancellationToken.None);
    }

    [Test]
    public void ThenThePatternIsMappedCorrectly()
    {
        var project = _result.Content.Projects[0];
        var pattern = project.Pattern;

        Assert.Multiple(() =>
        {
            Assert.That(project.PurchasedAt, Is.EqualTo(new DateTime(2023, 12, 07, 11, 19, 00)), nameof(project.PurchasedAt));

            Assert.That(pattern.Reference, Is.EqualTo(Guid.Parse("571099c0-be56-4a49-8dd6-1fb77c95cb04")), nameof(pattern.Reference));
            Assert.That(pattern.CreatedAt, Is.EqualTo(new DateTime(2023, 12, 12, 01, 33, 57)), nameof(pattern.CreatedAt));
            Assert.That(pattern.Title, Is.EqualTo("TestTitle"), nameof(pattern.Title));
            Assert.That(pattern.Width, Is.EqualTo(8773), nameof(pattern.Width));
            Assert.That(pattern.Height, Is.EqualTo(7725), nameof(pattern.Height));
            Assert.That(pattern.Price, Is.EqualTo(884.23m), nameof(pattern.Price));
            Assert.That(pattern.ThumbnailUrl, Is.EqualTo("TestThumbnailUrl"), nameof(pattern.ThumbnailUrl));
            Assert.That(pattern.ThreadCount, Is.EqualTo(6897), nameof(pattern.ThreadCount));
            Assert.That(pattern.StitchCount, Is.EqualTo(1446), nameof(pattern.StitchCount));
            Assert.That(pattern.BannerImageUrl, Is.EqualTo("TestBannerImageUrl"), nameof(pattern.BannerImageUrl));
            Assert.That(pattern.ExternalShopUrl, Is.EqualTo("TestExternalShopUrl"), nameof(pattern.ExternalShopUrl));
        });
    }
}