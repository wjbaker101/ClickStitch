using ClickStitch.Api.Projects.AddProject;
using ClickStitch.Api.Projects.AddProject.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;

namespace Api.Tests.Projects.AddProject;

[TestFixture]
[Parallelizable]
public sealed class GivenAnAddProjectRequest
{
    private UserRecord _user = null!;
    private PatternRecord _pattern = null!;

    private TestDatabase _database = null!;

    private Result<AddProjectResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _user = new UserRecord
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

        _pattern = new PatternRecord
        {
            Reference = Guid.Parse("d662b9c2-19a8-45b5-8587-7b07059a513a"),
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
            User = _user,
            Creator = new CreatorRecord
            {
                Id = 0,
                Reference = default,
                CreatedAt = default,
                Name = null!,
                StoreUrl = null!,
                Users = null!,
                Patterns = null!
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
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                _pattern
            }
        };

        var guidProvider = new FakeGuidProvider
        {
            FakeGuid = Guid.Parse("3c9bd2ca-aefe-4db6-b69c-0448ea3ef827")
        };

        var subject = new AddProjectService(new UserRepository(_database), new UserPatternRepository(_database), new PatternRepository(_database), new FakeDateTimeProvider
        {
            FakeUtcNow = new DateTime(2020, 04, 13, 23, 59, 02)
        }, guidProvider);

        _result = await subject.AddProject(new TestRequestUser(), Guid.Parse("d662b9c2-19a8-45b5-8587-7b07059a513a"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectProjectIsSaved()
    {
        var project = _database.Actions.Saved.OfType<UserPatternRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(project.User, Is.EqualTo(_user));
            Assert.That(project.Pattern, Is.EqualTo(_pattern));
            Assert.That(project.Reference, Is.EqualTo(Guid.Parse("3c9bd2ca-aefe-4db6-b69c-0448ea3ef827")));
            Assert.That(project.CreatedAt, Is.EqualTo(new DateTime(2020, 04, 13, 23, 59, 02)));
            Assert.That(project.PausePositionX, Is.Null);
            Assert.That(project.PausePositionY, Is.Null);
        });
    }
}