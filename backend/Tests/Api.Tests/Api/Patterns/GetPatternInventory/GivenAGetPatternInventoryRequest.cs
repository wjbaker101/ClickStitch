using ClickStitch.Api.Patterns.Services;
using ClickStitch.Api.Patterns.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Types;
using TestHelpers.Data;

namespace Api.Tests.Api.Patterns.GetPatternInventory;

[TestFixture]
[Parallelizable]
public sealed class GivenAGetPatternInventoryRequest
{
    private readonly Guid _patternReference = Guid.Parse("d8c0e6da-13a0-4800-94a2-9979db773732");

    private Result<GetPatternInventoryResponse> _result = null!;

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

        var thread1 = new ThreadRecord
        {
            Id = 4562,
            Reference = Guid.Parse("90dc9fc1-3fa5-4166-b797-b3a7ba8a3a27"),
            Brand = null!,
            Code = "TestName1",
            Colour = null!
        };

        var thread2 = new ThreadRecord
        {
            Id = 5917,
            Reference = Guid.Parse("6e565e4b-6ea2-47d8-82ab-014af606eb6c"),
            Brand = null!,
            Code = "TestName2",
            Colour = null!
        };

        var database = new TestDatabase
        {
            Records = new List<IDatabaseRecord>
            {
                user,
                new PatternRecord
                {
                    Reference = _patternReference,
                    CreatedAt = default,
                    Title = null,
                    Width = 0,
                    Height = 0,
                    Price = 0,
                    ThumbnailUrl = null,
                    ThreadCount = 0,
                    StitchCount = 0,
                    AidaCount = 0,
                    BannerImageUrl = null,
                    ExternalShopUrl = null,
                    TitleSlug = null,
                    IsPublic = false,
                    User = null,
                    Creator = null,
                    Threads = new HashSet<PatternThreadRecord>
                    {
                        new()
                        {
                            Pattern = null!,
                            Name = "Test TestName1",
                            Description = null!,
                            Index = 2432,
                            Colour = null!
                        },
                        new()
                        {
                            Pattern = null!,
                            Name = "Test  TestName2",
                            Description = null!,
                            Index = 5886,
                            Colour = null!
                        },
                        new()
                        {
                            Pattern = null!,
                            Name = "Test TestName3",
                            Description = null!,
                            Index = 4327,
                            Colour = null!
                        }
                    }
                },
                thread1,
                thread2,
                new UserThreadRecord
                {
                    User = user,
                    Thread = thread1,
                    Count = 8117
                }
            }
        };

        var subject = new GetPatternInventoryService(new UserRepository(database), new PatternRepository(database), new ThreadRepository(database), new UserThreadRepository(database));

        _result = await subject.GetPatternInventory(new TestRequestUser(), _patternReference, CancellationToken.None);
    }

    [Test]
    public void ThenTheResultIsASuccess()
    {
        Assert.That(_result.IsSuccess, Is.True);
    }

    [Test]
    public void ThenTheCorrectThreadsAreReturned()
    {
        var inventoryThreads = _result.Content.Threads;

        Assert.Multiple(() =>
        {
            Assert.That(inventoryThreads[2432]!.Thread.Reference, Is.EqualTo(Guid.Parse("90dc9fc1-3fa5-4166-b797-b3a7ba8a3a27")));
            Assert.That(inventoryThreads[2432]!.Count, Is.EqualTo(8117));

            Assert.That(inventoryThreads[5886]!.Thread.Reference, Is.EqualTo(Guid.Parse("6e565e4b-6ea2-47d8-82ab-014af606eb6c")));
            Assert.That(inventoryThreads[5886]!.Count, Is.EqualTo(0));

            Assert.That(inventoryThreads[4327], Is.Null);
        });
    }
}