using Core.Types;
using Data.Records;
using Data.Repositories.User;

namespace TestHelpers.Fakes;

public sealed class FakeUserRepository : FakeRepository<UserRecord>, IUserRepository
{
    private Result<UserRecord> _fakeResult;

    private FakeUserRepository() : base(new UserRecord 
    {
        Reference = Guid.Parse("2140f104-f614-47cf-ba30-9fe1d77f6f47"),
        CreatedAt = new DateTime(2023, 04, 16, 02, 55, 32),
        Email = "test@email.com",
        Password = "LV9DQ2T04FnF+OoBJQZHzGh+hABV2sSeIEp/2LtExU0=",
        PasswordSalt = "356ee6d2-7ace-4229-b467-bcded8797379"
    })
    {
        _fakeResult = FakeValue;
    }

    public static FakeUserRepository Default() => new();

    public static FakeUserRepository WithUser(Action<UserRecord> mutate)
    {
        var repository = new FakeUserRepository();
        mutate(repository._fakeResult.Content);
        return repository;
    }

    public static FakeUserRepository WithResult(Result result) => new()
    {
        _fakeResult = Result<UserRecord>.FromFailure(result)
    };

    public Task<UserRecord> GetByRequestUser(RequestUser requestUser, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult.Content);
    }

    public Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult);
    }

    public Task<Result<UserRecord>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult);
    }
}