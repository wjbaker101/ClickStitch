using Data.Records.Types;

namespace Data.Repositories.Admin;

public interface IAdminRepository
{
}

public sealed class AdminRepository : Repository<IDatabaseRecord>, IAdminRepository
{
    public AdminRepository(IDatabase database) : base(database)
    {
    }
}