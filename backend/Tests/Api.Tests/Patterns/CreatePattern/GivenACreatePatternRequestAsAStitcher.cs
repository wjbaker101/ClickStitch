using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Parsing.Types;
using ClickStitch.Api.Patterns.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using DotNetLibs.Core.Services.Fakes;
using TestHelpers.Data;

namespace Api.Tests.Patterns.CreatePattern;

[TestFixture]
[Parallelizable]
public sealed class GivenACreatePatternRequestAsAStitcher
{
    private UserRecord _user = null!;

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

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user
            }
        };

        _cloudinary = new FakeCloudinaryClient();

        var patternParserService = new FakePatternParserService
        {
            Response = new ParsePatternResponse
            {
                Pattern = new ParsePatternResponse.PatternDetails
                {
                    Width = 3,
                    Height = 1,
                    ThreadCount = 2769,
                    StitchCount = 4402
                },
                Threads = new List<ParsePatternResponse.ThreadDetails>
                {
                    new()
                    {
                        Name = "TestName1",
                        Description = "TestDescription1",
                        Index = 2470,
                        Colour = "#ff0000"
                    },
                    new()
                    {
                        Name = "TestName2",
                        Description = "TestDescription2",
                        Index = 9637,
                        Colour = "#00ff00"
                    },
                    new()
                    {
                        Name = "TestName3",
                        Description = "TestDescription3",
                        Index = 8142,
                        Colour = "#0000ff"
                    }
                },
                Stitches = new List<ParsePatternResponse.StitchDetails>
                {
                    new()
                    {
                        ThreadIndex = 2470,
                        X = 0,
                        Y = 0
                    },
                    new()
                    {
                        ThreadIndex = 9637,
                        X = 1,
                        Y = 0
                    },
                    new()
                    {
                        ThreadIndex = 8142,
                        X = 2,
                        Y = 0
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
            Permissions = new List<RequestPermissionType>()
        };

        var request = new CreatePatternRequest
        {
            Title = "Test Title",
            Price = 2189,
            AidaCount = 9606,
            ExternalShopUrl = "TestExternalShopUrl"
        };

        var subject = new PatternsService(
            new PatternRepository(_database),
            new PatternThreadRepository(_database),
            new PatternUploadService(_cloudinary),
            new UserRepository(_database),
            new UserPatternRepository(_database),
            new CreatorRepository(_database),
            new PatternThreadStitchRepository(_database),
            patternParserService,
            guidProvider);

        _result = await subject.CreatePattern(requestUser, request, "", null!, null, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheBannerImageIsUploaded()
    {
        var bannerImage = new byte[]
        {
            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 3, 0, 0, 0, 1, 8, 6, 0, 0, 0, 27, 224, 20, 180, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 196,
            0, 0, 14, 196, 1, 149, 43, 14, 27, 0, 0, 0, 18, 73, 68, 65, 84, 120, 156, 99, 249, 207, 192, 240, 159, 17, 72, 48, 0, 9, 0, 30, 46, 4, 3, 242, 58, 185, 235, 0, 0,
            0, 0, 73, 69, 78, 68, 174, 66, 96, 130
        };

        Assert.That(_cloudinary.ActualRequest.FileName, Is.EqualTo("patterns/f55f0dde-4841-4444-b86d-5d7490b8f636/banner"));
        Assert.That(_cloudinary.ActualRequest.FileContents.ToBytes(), Is.EqualTo(bannerImage));
    }

    [Test]
    public void ThenTheCorrectPatternIsSaved()
    {
        var pattern = _database.Actions.Saved.OfType<PatternRecord>().Single();

        Assert.Multiple(() =>
        {
            Assert.That(pattern.Title, Is.EqualTo("Test Title"), nameof(pattern.Title));
            Assert.That(pattern.Width, Is.EqualTo(3), nameof(pattern.Width));
            Assert.That(pattern.Height, Is.EqualTo(1), nameof(pattern.Height));
            Assert.That(pattern.Price, Is.EqualTo(2189), nameof(pattern.Price));
            Assert.That(pattern.ThumbnailUrl, Is.EqualTo(""), nameof(pattern.ThumbnailUrl));
            Assert.That(pattern.ThreadCount, Is.EqualTo(2769), nameof(pattern.ThreadCount));
            Assert.That(pattern.StitchCount, Is.EqualTo(4402), nameof(pattern.StitchCount));
            Assert.That(pattern.AidaCount, Is.EqualTo(9606), nameof(pattern.AidaCount));
            Assert.That(pattern.BannerImageUrl, Is.EqualTo("TestImageUrl"), nameof(pattern.BannerImageUrl));
            Assert.That(pattern.ExternalShopUrl, Is.EqualTo("TestExternalShopUrl"), nameof(pattern.ExternalShopUrl));
            Assert.That(pattern.TitleSlug, Is.EqualTo("test-title"), nameof(pattern.TitleSlug));
            Assert.That(pattern.IsPublic, Is.False, nameof(pattern.IsPublic));
            Assert.That(pattern.User, Is.EqualTo(_user), nameof(pattern.User));
            Assert.That(pattern.Creator, Is.Null, nameof(pattern.Creator));
            Assert.That(pattern.Threads, Is.Empty, nameof(pattern.Threads));
        });
    }

    [Test]
    public void ThenTheCorrectThreadsAreSaved()
    {
        var threads = _database.Actions.Saved.OfType<PatternThreadRecord>().ToList();
        var thread1 = threads[0];
        var thread2 = threads[1];
        var thread3 = threads[2];

        Assert.Multiple(() =>
        {
            Assert.That(thread1.Name, Is.EqualTo("TestName1"), nameof(thread1.Name));
            Assert.That(thread1.Description, Is.EqualTo("TestDescription1"), nameof(thread1.Description));
            Assert.That(thread1.Index, Is.EqualTo(2470), nameof(thread1.Index));
            Assert.That(thread1.Colour, Is.EqualTo("#ff0000"), nameof(thread1.Colour));

            Assert.That(thread2.Name, Is.EqualTo("TestName2"), nameof(thread1.Name));
            Assert.That(thread2.Description, Is.EqualTo("TestDescription2"), nameof(thread1.Description));
            Assert.That(thread2.Index, Is.EqualTo(9637), nameof(thread1.Index));
            Assert.That(thread2.Colour, Is.EqualTo("#00ff00"), nameof(thread1.Colour));

            Assert.That(thread3.Name, Is.EqualTo("TestName3"), nameof(thread1.Name));
            Assert.That(thread3.Description, Is.EqualTo("TestDescription3"), nameof(thread1.Description));
            Assert.That(thread3.Index, Is.EqualTo(8142), nameof(thread1.Index));
            Assert.That(thread3.Colour, Is.EqualTo("#0000ff"), nameof(thread1.Colour));
        });
    }

    //[Test]
    //public void ThenTheCorrectStitchesAreSaved()
    //{
    //    var stitches = _database.Actions.Saved.OfType<PatternThreadStitchRecord>().ToList();
    //    var stitch1 = stitches[0];
    //    var stitch2 = stitches[1];
    //    var stitch3 = stitches[2];

    //    Assert.Multiple(() =>
    //    {
    //        Assert.That(stitch1.Thread.Index, Is.EqualTo(2470), nameof(stitch1.Thread.Index));
    //        Assert.That(stitch1.X, Is.EqualTo(0), nameof(stitch1.X));
    //        Assert.That(stitch1.Y, Is.EqualTo(0), nameof(stitch1.Y));

    //        Assert.That(stitch2.Thread.Index, Is.EqualTo(9637), nameof(stitch1.Thread.Index));
    //        Assert.That(stitch2.X, Is.EqualTo(1), nameof(stitch1.X));
    //        Assert.That(stitch2.Y, Is.EqualTo(0), nameof(stitch1.Y));

    //        Assert.That(stitch3.Thread.Index, Is.EqualTo(8142), nameof(stitch1.Thread.Index));
    //        Assert.That(stitch3.X, Is.EqualTo(2), nameof(stitch1.X));
    //        Assert.That(stitch3.Y, Is.EqualTo(0), nameof(stitch1.Y));
    //    });
    //}

    [Test]
    public void ThenTheProjectIsSaved()
    {
        var project = _database.Actions.Saved.OfType<UserPatternRecord>().Single();
        var pattern = _database.Actions.Saved.OfType<PatternRecord>().Single();

        Assert.That(project.User, Is.EqualTo(_user), nameof(project.User));
        Assert.That(project.Pattern, Is.EqualTo(pattern), nameof(project.Pattern));
        Assert.That(project.PausePositionX, Is.Null, nameof(project.PausePositionX));
        Assert.That(project.PausePositionY, Is.Null, nameof(project.PausePositionY));
    }
}