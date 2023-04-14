using Data.Records;

namespace Data.Repositories.Permission;

public interface IPermissionRepository : IRepository<PermissionRecord>
{
}

public sealed class PermissionRepository : Repository<PermissionRecord>, IPermissionRepository
{
    public PermissionRepository(IDatabase database) : base(database)
    {
    }
}