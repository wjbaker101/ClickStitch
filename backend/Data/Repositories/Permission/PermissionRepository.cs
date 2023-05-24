namespace Data.Repositories.Permission;

public interface IPermissionRepository : IRepository<PermissionRecord>
{
    Task<Result<PermissionRecord>> GetByType(PermissionType permissionType, CancellationToken cancellationToken);
}

public sealed class PermissionRepository : Repository<PermissionRecord>, IPermissionRepository
{
    public PermissionRepository(IDatabase database) : base(database)
    {
    }

    public async Task<Result<PermissionRecord>> GetByType(PermissionType permissionType, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var permission = await session
            .Query<PermissionRecord>()
            .SingleOrDefaultAsync(x => x.Type == permissionType, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        if (permission == null)
            return Result<PermissionRecord>.Failure($"Unable to find permission with type: '{permissionType}'.");

        return permission;
    }
}