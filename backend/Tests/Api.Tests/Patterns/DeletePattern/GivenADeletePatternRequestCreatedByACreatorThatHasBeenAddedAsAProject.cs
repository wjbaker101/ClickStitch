﻿using ClickStitch.Api.Patterns.DeletePattern;
using ClickStitch.Api.Patterns.DeletePattern.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Patterns.DeletePattern;

[TestFixture]
[Parallelizable]
public sealed class GivenADeletePatternRequestCreatedByACreatorThatHasBeenAddedAsAProject
{
    private Result<DeletePatternResponse> _result = null!;

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

        var pattern = new PatternRecord
        {
            Reference = Guid.Parse("6f6f9364-d204-4bdb-813f-a950938163b8"),
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
            User = user,
            Creator = new CreatorRecord
            {
                Reference = default,
                CreatedAt = default,
                Name = null,
                StoreUrl = null,
                Description = null!,
                Users = null,
                Patterns = null
            },
            Threads = null!
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                pattern,
                new UserPatternRecord
                {
                    Id = 0,
                    User = null!,
                    Pattern = pattern,
                    Reference = default,
                    CreatedAt = default,
                    PausePositionX = null,
                    PausePositionY = null
                }
            }
        };

        var subject = new DeletePatternService(new PatternRepository(database), null!, new UserRepository(database), new UserPatternRepository(database));

        _result = await subject.DeletePattern(new TestRequestUser(), Guid.Parse("6f6f9364-d204-4bdb-813f-a950938163b8"), CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectMessageIsReturned()
    {
        Assert.That(_result.Content.Message, Is.EqualTo("At least 1 user had this pattern, so it has been marked as deleted. It still exists, but won't show up for new users."));
    }
}