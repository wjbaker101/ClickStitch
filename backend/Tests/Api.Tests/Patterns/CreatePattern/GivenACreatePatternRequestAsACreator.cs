using ClickStitch.Api.Patterns.CreatePattern;
using ClickStitch.Api.Patterns.CreatePattern.Parsing.Types;
using ClickStitch.Api.Patterns.CreatePattern.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;

namespace Api.Tests.Patterns.CreatePattern;

[TestFixture]
[Parallelizable]
public sealed class GivenACreatePatternRequestAsACreator
{
    private UserRecord _user = null!;
    private CreatorRecord _creator = null!;

    private TestDatabase _database = null!;
    private FakeCloudinaryClient _cloudinary = null!;

    private Result _result = null!;

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

        _creator = new CreatorRecord
        {
            Reference = default,
            CreatedAt = default,
            Name = null!,
            StoreUrl = null!,
            Users = null!,
            Patterns = null!
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                new UserCreatorRecord
                {
                    User = _user,
                    Creator = _creator
                }
            }
        };

        _cloudinary = new FakeCloudinaryClient();

        var patternParserService = new FakePatternParserService
        {
            Response = new ParsePatternResponse
            {
                Pattern = new ParsePatternResponse.PatternDetails
                {
                    Width = 673,
                    Height = 213,
                    ThreadCount = 2769,
                    StitchCount = 4402
                },
                Threads = new List<ParsePatternResponse.ThreadDetails>
                {
                    new()
                    {
                        Name = "TestName",
                        Description = "TestDescription",
                        Index = 2470,
                        Colour = "TestColour"
                    }
                },
                Stitches = new List<ParsePatternResponse.StitchDetails>
                {
                    new()
                    {
                        ThreadIndex = 2470,
                        X = 68,
                        Y = 22
                    }
                },
                BackStitches = new List<ParsePatternResponse.BackStitchDetails>
                {
                    new()
                    {
                        ThreadIndex = 2470,
                        StartX = 882,
                        StartY = 501,
                        EndX = 79,
                        EndY = 267
                    }
                }
            }
        };

        var guidProvider = new FakeGuidProvider
        {
            FakeGuid = Guid.Parse("f55f0dde-4841-4444-b86d-5d7490b8f636")
        };

        var requestUser = new TestRequestUser
        {
            Permissions = new List<RequestPermissionType>
            {
                RequestPermissionType.Creator
            }
        };

        var request = new CreatePatternRequest
        {
            Title = "Test Title",
            Price = 2189,
            AidaCount = 9606,
            ExternalShopUrl = "TestExternalShopUrl"
        };

        var subject = new CreatePatternService(
            new PatternRepository(_database),
            new PatternThreadRepository(_database),
            new PatternUploadService(_cloudinary),
            new UserRepository(_database),
            null!,
            new CreatorRepository(_database),
            patternParserService,
            guidProvider);

        _result = await subject.CreatePattern(requestUser, request, "", null!, new FakeFormFile(), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheBannerImageIsUploaded()
    {
        Assert.That(_cloudinary.ActualFileName, Is.EqualTo("patterns/f55f0dde-4841-4444-b86d-5d7490b8f636/banner"));
        Assert.That(_cloudinary.ActualFileContents, Is.EqualTo(new byte[] { 1, 2, 3 }));
    }

    [Test]
    public void ThenTheCorrectPatternIsSaved()
    {
        var pattern = _database.Actions.Saved.OfType<PatternRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(pattern.Title, Is.EqualTo("Test Title"), nameof(pattern.Title));
            Assert.That(pattern.Width, Is.EqualTo(673), nameof(pattern.Width));
            Assert.That(pattern.Height, Is.EqualTo(213), nameof(pattern.Height));
            Assert.That(pattern.Price, Is.EqualTo(2189), nameof(pattern.Price));
            Assert.That(pattern.ThumbnailUrl, Is.EqualTo(""), nameof(pattern.ThumbnailUrl));
            Assert.That(pattern.ThreadCount, Is.EqualTo(2769), nameof(pattern.ThreadCount));
            Assert.That(pattern.StitchCount, Is.EqualTo(4402), nameof(pattern.StitchCount));
            Assert.That(pattern.AidaCount, Is.EqualTo(9606), nameof(pattern.AidaCount));
            Assert.That(pattern.BannerImageUrl, Is.EqualTo("TestImageUrl"), nameof(pattern.BannerImageUrl));
            Assert.That(pattern.ExternalShopUrl, Is.EqualTo("TestExternalShopUrl"), nameof(pattern.ExternalShopUrl));
            Assert.That(pattern.TitleSlug, Is.EqualTo("test-title"), nameof(pattern.TitleSlug));
            Assert.That(pattern.IsPublic, Is.True, nameof(pattern.IsPublic));
            Assert.That(pattern.User, Is.EqualTo(_user), nameof(pattern.User));
            Assert.That(pattern.Creator, Is.EqualTo(_creator), nameof(pattern.Creator));
            Assert.That(pattern.Threads, Is.Empty, nameof(pattern.Threads));
        });
    }

    [Test]
    public void ThenTheCorrectThreadsAreSaved()
    {
        var thread = _database.Actions.Saved.OfType<PatternThreadRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(thread.Name, Is.EqualTo("TestName"), nameof(thread.Name));
            Assert.That(thread.Description, Is.EqualTo("TestDescription"), nameof(thread.Description));
            Assert.That(thread.Index, Is.EqualTo(2470), nameof(thread.Index));
            Assert.That(thread.Colour, Is.EqualTo("TestColour"), nameof(thread.Colour));
            Assert.That(thread.Stitches, Is.EqualTo(new int[][] { [68, 22] }), nameof(thread.Stitches));
            Assert.That(thread.BackStitches, Is.EqualTo(new int[][] { [882, 501, 79, 267] }), nameof(thread.Stitches));
        });
    }
}