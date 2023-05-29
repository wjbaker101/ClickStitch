using Core.Types;
using Data.Records;
using Data.Repositories.Creator;

namespace TestHelpers.Fakes;

public sealed class FakeCreatorRepository : FakeRepository<CreatorRecord>, ICreatorRepository
{
    private Result<CreatorRecord> _fakeResult;

    private FakeCreatorRepository() : base(new CreatorRecord
    {
        Reference = Guid.Parse("2140f104-f614-47cf-ba30-9fe1d77f6f47"),
        CreatedAt = new DateTime(2023, 04, 16, 02, 55, 32),
        Name = "TestCreatorName",
        StoreUrl = "TestStoreUrl",
        Users = new List<UserRecord>(),
        Patterns = new List<PatternRecord>()
    })
    {
        _fakeResult = FakeValue;
    }

    public static FakeCreatorRepository Default() => new();

    public static FakeCreatorRepository WithCreator(Action<CreatorRecord> mutate)
    {
        var repository = new FakeCreatorRepository();
        mutate(repository._fakeResult.Content);
        return repository;
    }

    public static FakeCreatorRepository WithResult(Result result) => new()
    {
        _fakeResult = Result<CreatorRecord>.FromFailure(result)
    };

    public Task<Result<CreatorRecord>> GetFullByReference(Guid creatorReference, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult);
    }

    public Task<Result<CreatorRecord>> GetWithUsersByReference(Guid creatorReference, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult);
    }

    public Task<Result<CreatorRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult);
    }
}