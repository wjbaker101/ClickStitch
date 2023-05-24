namespace Data.Repositories.Creator;

public interface ICreatorRepository : IRepository<CreatorRecord>
{
}

public sealed class CreatorRepository : Repository<CreatorRecord>, ICreatorRepository
{
    public CreatorRepository(IDatabase database) : base(database)
    {
    }
}