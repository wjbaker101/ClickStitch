using ClickStitch.Api.Patterns;
using ClickStitch.Api.Patterns.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.User;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Patterns.UpdatePattern;

[TestFixture]
[Parallelizable]
public sealed class GivenAnUpdatePatternRequestWithADifferentUser
{
    private readonly Guid _userReference = Guid.Parse("98b794fc-debb-43b1-94f4-8d7cbc803cc3");
    private readonly Guid _patternReference = Guid.Parse("97d9d9df-d3e3-42a5-b03d-3a6fdd35df22");

    private TestDatabase _database = null!;

    private Result<UpdatePatternResponse> _result = null!;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var user = new UserRecord
        {
            Id = 5023,
            Reference = _userReference,
            CreatedAt = default,
            Email = null,
            Password = null,
            PasswordSalt = null,
            LastLoginAt = null,
            Permissions = null
        };

        _database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new PatternRecord
                {
                    Reference = _patternReference,
                    CreatedAt = default,
                    Title = "NewTestTitle",
                    Width = 0,
                    Height = 0,
                    Price = 0,
                    ThumbnailUrl = null,
                    ThreadCount = 0,
                    StitchCount = 0,
                    AidaCount = 0,
                    BannerImageUrl = null,
                    ExternalShopUrl = "NewTestExternalShopUrl",
                    TitleSlug = null,
                    IsPublic = false,
                    User = new UserRecord
                    {
                        Id = 2613,
                        Reference = default,
                        CreatedAt = default,
                        Email = null,
                        Password = null,
                        PasswordSalt = null,
                        LastLoginAt = null,
                        Permissions = null
                    },
                    Creator = null,
                    Threads = null
                }
            }
        };

        var requestUser = new TestRequestUser
        {
            Reference = _userReference
        };

        var request = new UpdatePatternRequest
        {
            Title = "NewTestTitle",
            ExternalShopUrl = "NewTestExternalShopUrl"
        };

        var subject = new PatternsService(new PatternRepository(_database), null!, null!, new UserRepository(_database), null!, null!, null!, null!);

        _result = await subject.UpdatePattern(requestUser, _patternReference, request, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsAFailure()
    {
        Assert.That(_result.IsFailure, Is.True);
    }

    [Test]
    public void ThenTheCorrectErrorIsReturned()
    {
        Assert.That(_result.FailureMessage, Is.EqualTo("Unable to update pattern as you are not a creator of it."));
    }
}