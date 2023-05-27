using Core.Types;
using Data.Records;
using Data.Repositories.UserPattern;

namespace TestHelpers.Fakes;

public sealed class FakeUserPatternRepository : FakeRepository<UserPatternRecord>, IUserPatternRepository
{
    private Result<List<UserPatternRecord>> _fakeResult;

    public FakeUserPatternRepository() : base(new UserPatternRecord
    {
        User = null!,
        Pattern = new PatternRecord
        {
            Reference = Guid.Parse("571099c0-be56-4a49-8dd6-1fb77c95cb04"),
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
            Creator = new CreatorRecord
            {
                Reference = Guid.Parse("0a30e82e-b2f3-47be-bba1-f6a4370f66ba"),
                CreatedAt = new DateTime(2021, 11, 02, 16, 55, 09),
                Name = "TestCreatorName",
                StoreUrl = "TestStoreUrl",
                Users = new List<UserRecord>(),
                Patterns = new List<PatternRecord>()
            },
            Stitches = new HashSet<PatternStitchRecord>
            {
                new()
                {
                    Pattern = null!,
                    ThreadIndex = 8533,
                    X = 2061,
                    Y = 9222
                }
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
        },
        CreatedAt = new DateTime(2023, 12, 07, 11, 19, 00)
    })
    {
        _fakeResult = new List<UserPatternRecord>
        {
            FakeValue
        };
    }

    public static FakeUserPatternRepository Default() => new();

    public static FakeUserPatternRepository WithUser(Action<List<UserPatternRecord>> mutate)
    {
        var repository = new FakeUserPatternRepository();
        mutate(repository._fakeResult.Content);
        return repository;
    }

    public static FakeUserPatternRepository WithResult(Result result) => new()
    {
        _fakeResult = Result<List<UserPatternRecord>>.FromFailure(result)
    };

    public Task<List<UserPatternRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult.Content);
    }

    public Task<Result<UserPatternRecord>> GetByUserAndPatternAsync(UserRecord user, PatternRecord pattern, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UserPatternRecord>>(_fakeResult.Content[0]);
    }
}