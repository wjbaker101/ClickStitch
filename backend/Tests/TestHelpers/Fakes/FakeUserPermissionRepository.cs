using Core.Types;
using Data.Records;
using Data.Repositories.UserPermission;

namespace TestHelpers.Fakes;

public sealed class FakeUserPermissionRepository : FakeRepository<UserPermissionRecord>, IUserPermissionRepository
{
    private Result<List<PermissionRecord>> _fakeResult;

    private FakeUserPermissionRepository() : base(new UserPermissionRecord
    {
        User = null,
        Permission = new PermissionRecord
        {
            Type = PermissionType.Admin,
            Name = "Admin"
        },
        CreatedAt = new DateTime(2023, 01, 14, 22, 19, 02)
    })
    {
        _fakeResult = new List<PermissionRecord>
        {
            FakeValue.Permission
        };
    }

    public static FakeUserPermissionRepository Default() => new();

    public static FakeUserPermissionRepository WithUser(Action<List<PermissionRecord>> mutate)
    {
        var repository = new FakeUserPermissionRepository();
        mutate(repository._fakeResult.Content);
        return repository;
    }

    public static FakeUserPermissionRepository WithResult(Result result) => new()
    {
        _fakeResult = Result<List<PermissionRecord>>.FromFailure(result)
    };

    public Task<List<PermissionRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResult.Content);
    }

    public Task<Result<UserPermissionRecord>> GetByUserAndPermission(UserRecord user, PermissionRecord permission, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UserPermissionRecord>>(null!);
    }
}