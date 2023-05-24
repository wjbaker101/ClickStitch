namespace Data.Repositories.UserCreator;

public interface IUserCreatorRepository : IRepository<UserCreatorRecord>
{
}

public sealed class UserCreatorRepository : Repository<UserCreatorRecord>, IUserCreatorRepository
{
    public UserCreatorRepository(IDatabase database) : base(database)
    {
    }
}