﻿using ClickStitch.Api.Projects.CompleteStitches.Types;
using ClickStitch.Api.Projects.UnCompleteStitches;
using Data.Records;
using Data.Repositories.User;
using Data.Repositories.UserPatternThreadStitch;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Projects.UnCompleteStitches;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUnCompleteStitchesRequest
{
    private UserRecord _user = null!;
    private UserPatternThreadStitchRecord _stitch1 = null!;
    private UserPatternThreadStitchRecord _stitch2 = null!;
    private UserPatternThreadStitchRecord _stitch3 = null!;
    private UserPatternThreadStitchRecord _stitch4 = null!;

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

        _stitch1 = new UserPatternThreadStitchRecord
        {
            User = _user,
            Thread = null!,
            X = default,
            Y = default,
            CompletedAt = default
        };

        _stitch2 = new UserPatternThreadStitchRecord
        {
            User = _user,
            Thread = null!,
            X = default,
            Y = default,
            CompletedAt = default
        };

        _stitch3 = new UserPatternThreadStitchRecord
        {
            User = _user,
            Thread = null!,
            X = default,
            Y = default,
            CompletedAt = default
        };

        _stitch4 = new UserPatternThreadStitchRecord
        {
            User = _user,
            Thread = null!,
            X = default,
            Y = default,
            CompletedAt = default
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                _user,
                thread1,
                thread2,
                _stitch1,
                _stitch2,
                _stitch3,
                _stitch4
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

        var subject = new UnCompleteStitchesService(new UserRepository(_database), new UserPatternThreadStitchRepository(_database));

        _result = await subject.UnCompleteStitches(new TestRequestUser(), Guid.Parse("d5925542-128c-40b4-86a1-b8dcddc848f7"), request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }
}