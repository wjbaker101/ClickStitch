using ClickStitch.Api.Projects.CompleteStitches;
using ClickStitch.Api.Projects.CompleteStitches.Types;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadStitch;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.CompleteStitches;

[TestFixture]
[Parallelizable]
public sealed class GivenACompleteStitchesRequest
{
    private UserRecord _user = null!;

    private TestDatabase _database = null!;

    private Result<CompleteStitchesResponse> _result = null!;

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

        var thread1 = new PatternThreadRecord
        {
            Pattern = new PatternRecord
            {
                Reference = Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"),
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
            },
            Name = null!,
            Description = null!,
            Index = 1,
            Colour = null!,
            Stitches = [],
            BackStitches = []
        };

        var thread2 = new PatternThreadRecord
        {
            Pattern = new PatternRecord
            {
                Reference = Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"),
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
            },
            Name = null!,
            Description = null!,
            Index = 2,
            Colour = null!,
            Stitches = [],
            BackStitches = []
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                thread1,
                thread2
            }
        };

        var request = new CompleteStitchesRequest
        {
            StitchesByThread = new Dictionary<int, List<CompleteStitchesRequest.Position>>
            {
                [1] = new()
                {
                    new CompleteStitchesRequest.Position { X = 1, Y = 1 },
                    new CompleteStitchesRequest.Position { X = 2, Y = 2 }
                },
                [2] = new()
                {
                    new CompleteStitchesRequest.Position { X = 3, Y = 3 },
                    new CompleteStitchesRequest.Position { X = 4, Y = 4 }
                }
            }
        };

        var subject = new CompleteStitchesService(new UserRepository(_database), new UserPatternThreadStitchRepository(_database));

        _result = await subject.CompleteStitches(new TestRequestUser(), Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"), request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }
}