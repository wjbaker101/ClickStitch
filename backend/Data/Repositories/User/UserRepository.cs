using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.User;

public interface IUserRepository : IRepository<UserRecord>
{
    Result<UserRecord> GetByReference(Guid userReference);
    Result<UserRecord> GetByUsername(string username);
    List<PatternRecord> GetProjectsByUser(UserRecord user);
}

public sealed class UserRepository : Repository<UserRecord>, IUserRepository
{
    public UserRepository(IDatabase database) : base(database)
    {
    }

    public Result<UserRecord> GetByReference(Guid userReference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = session
            .Query<UserRecord>()
            .SingleOrDefault(x => x.Reference == userReference);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with reference: '{userReference}'.");

        transaction.Commit();

        return user;
    }

    public Result<UserRecord> GetByUsername(string username)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = session
            .Query<UserRecord>()
            .SingleOrDefault(x => x.Username == username);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with username: '{username}'.");

        transaction.Commit();

        return user;
    }

    public List<PatternRecord> GetProjectsByUser(UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var patterns = session
            .Query<UserRecord>()
            .FetchMany(x => x.Patterns)
            .Where(x => x == user)
            .SelectMany(x => x.Patterns)
            .ToList();

        transaction.Commit();

        return patterns;
    }
}