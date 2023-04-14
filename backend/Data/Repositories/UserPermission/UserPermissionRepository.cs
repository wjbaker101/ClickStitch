using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.UserPermission;

public interface IUserPermissionRepository : IRepository<UserPermissionRecord>
{
    Task<List<PermissionRecord>> GetByUser(UserRecord user);
}

public sealed class UserPermissionRepository : Repository<UserPermissionRecord>, IUserPermissionRepository
{
    public UserPermissionRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<PermissionRecord>> GetByUser(UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var permissions = await session
            .Query<UserPermissionRecord>()
            .Fetch(x => x.Permission)
            .Where(x => x.User == user)
            .Select(x => x.Permission)
            .ToListAsync();

        await transaction.CommitAsync();

        return permissions;
    }
}