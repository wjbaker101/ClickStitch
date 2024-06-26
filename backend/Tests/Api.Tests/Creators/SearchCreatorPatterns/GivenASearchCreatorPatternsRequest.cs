﻿using ClickStitch.Api.Creators.SearchCreatorPatterns;
using ClickStitch.Api.Creators.SearchCreatorPatterns.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using Data.Repositories.UserPattern;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Creators.SearchCreatorPatterns;

[TestFixture]
[Parallelizable]
public sealed class GivenASearchCreatorPatternsRequest
{
    private readonly Guid _creatorReference = Guid.Parse("0292d848-4d41-4a69-8e48-af5e9f93b48c");

    private Result<SearchCreatorPatternsResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var creator = new CreatorRecord
        {
            Reference = _creatorReference,
            CreatedAt = default,
            Name = null!,
            StoreUrl = null!,
            Description = null!,
            Users = null!,
            Patterns = null!
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                new UserRecord
                {
                    Id = TestRequestUser.USER_ID,
                    Reference = default,
                    CreatedAt = default,
                    Email = null,
                    Password = null,
                    PasswordSalt = null,
                    LastLoginAt = null,
                    Permissions = null
                },
                creator,
                new PatternRecord
                {
                    Reference = Guid.Parse("e032f8bb-014d-4bf1-9a4d-dff526cf483f"),
                    CreatedAt = new DateTime(2010, 05, 03),
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
                    User = new UserRecord
                    {
                        Reference = default,
                        CreatedAt = default,
                        Email = null,
                        Password = null,
                        PasswordSalt = null,
                        LastLoginAt = null,
                        Permissions = null
                    },
                    Creator = creator,
                    Threads = null!
                },
                new PatternRecord
                {
                    Reference = Guid.Parse("47e5af1c-b051-4e34-8e97-8afa51e3f66a"),
                    CreatedAt = new DateTime(2010, 05, 01),
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
                    User = new UserRecord
                    {
                        Reference = default,
                        CreatedAt = default,
                        Email = null,
                        Password = null,
                        PasswordSalt = null,
                        LastLoginAt = null,
                        Permissions = null
                    },
                    Creator = new CreatorRecord
                    {
                        Reference = default,
                        CreatedAt = default,
                        Name = null!,
                        StoreUrl = null!,
                        Description = null!,
                        Users = null!,
                        Patterns = null!
                    },
                    Threads = null!
                },
                new PatternRecord
                {
                    Reference = Guid.Parse("baed58ae-9e88-4138-bdc9-09761ef1a0ec"),
                    CreatedAt = new DateTime(2010, 05, 02),
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
                    User = new UserRecord
                    {
                        Reference = default,
                        CreatedAt = default,
                        Email = null,
                        Password = null,
                        PasswordSalt = null,
                        LastLoginAt = null,
                        Permissions = null
                    },
                    Creator = creator,
                    Threads = null!
                }
            }
        };

        var subject = new SearchCreatorPatternsService(new CreatorRepository(database), new UserRepository(database), new UserPatternRepository(database));

        _result = await subject.SearchCreatorPatterns(new TestRequestUser(), _creatorReference, 10, 1, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectPatternsAreReturned()
    {
        Assert.That(_result.Content.Patterns[0].Reference, Is.EqualTo(Guid.Parse("e032f8bb-014d-4bf1-9a4d-dff526cf483f")));
        Assert.That(_result.Content.Patterns[1].Reference, Is.EqualTo(Guid.Parse("baed58ae-9e88-4138-bdc9-09761ef1a0ec")));
    }

    [Test]
    public void ThenTheCorrectNumberOfPatternsPatternsAreReturned()
    {
        Assert.That(_result.Content.Patterns.Count, Is.EqualTo(2));
        Assert.That(_result.Content.Pagination.TotalCount, Is.EqualTo(2));
    }
}