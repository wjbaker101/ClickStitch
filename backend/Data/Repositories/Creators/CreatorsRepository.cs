namespace Data.Repositories.Creators;

public interface ICreatorsRepository : IRepository<CreatorRecord>
{
}

public sealed class CreatorsRepository : Repository<CreatorRecord>, ICreatorsRepository
{
    public CreatorsRepository(IDatabase database) : base(database)
    {
    }
}