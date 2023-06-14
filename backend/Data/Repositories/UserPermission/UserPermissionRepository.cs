namespace Data.Repositories.UserPermission;

public interface IUserPermissionRepository : IRepository<UserPermissionRecord>
{
    Task<List<PermissionRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken);
    Task<Result<UserPermissionRecord>> GetByUserAndPermission(UserRecord user, PermissionRecord permission, CancellationToken cancellationToken);
}

public sealed class UserPermissionRepository : Repository<UserPermissionRecord>, IUserPermissionRepository
{
    public UserPermissionRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<PermissionRecord>> GetByUser(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var permissions = await session
            .Query<UserPermissionRecord>()
            .Fetch(x => x.Permission)
            .Where(x => x.User == user)
            .Select(x => x.Permission)
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return permissions;
    }

    public async Task<Result<UserPermissionRecord>> GetByUserAndPermission(UserRecord user, PermissionRecord permission, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var userPermission = await session
            .Query<UserPermissionRecord>()
            .SingleOrDefaultAsync(x => x.User == user && x.Permission == permission, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (userPermission is null)
            return Result<UserPermissionRecord>.Failure($"Cannot find user permission for user: '{user.Reference}' and permission: '{permission.Type}'.");

        return userPermission;
    }
}