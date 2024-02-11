using ClickStitch.Api.Projects;
using ClickStitch.Api.Projects.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.AddProject;

[TestFixture]
[Parallelizable]
public sealed class GivenAnAddProjectRequestForAnInvalidPatternReference
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
            Threads = new HashSet<PatternThreadRecord>()
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                _pattern
            }
        };

        var subject = new ProjectsService(new UserRepository(_database), new UserPatternRepository(_database), new PatternRepository(_database), null!, null!, null!);

        _result = await subject.AddProject(new TestRequestUser(), Guid.Parse("e27c961c-ca2d-4aff-abfe-f61935735c9d"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorMessageIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo($"Unable to find pattern with reference: 'e27c961c-ca2d-4aff-abfe-f61935735c9d'."));
    }
}