using Data.Records;

namespace Data.Repositories.UserPermission;

public interface IUserPermissionRepository : IRepository<UserPermissionRecord>
{
}

public sealed class UserPermissionRepository : Repository<UserPermissionRecord>, IUserPermissionRepository
{
    public UserPermissionRepository(IDatabase database) : base(database)
    {
    }
}