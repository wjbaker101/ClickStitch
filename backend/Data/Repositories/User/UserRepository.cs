using Core.Types;
using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.User;

public interface IUserRepository : IRepository<UserRecord>
{
    Result<UserRecord> GetByReference(Guid userReference);
    Result<UserRecord> GetByUsername(string username);
    Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference);
    Task<Result<UserRecord>> GetByUsernameAsync(string username);
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

    public async Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Reference == userReference);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with reference: '{userReference}'.");

        await transaction.CommitAsync();

        return user;
    }

    public async Task<Result<UserRecord>> GetByUsernameAsync(string username)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Username == username);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with username: '{username}'.");

        await transaction.CommitAsync();

        return user;
    }
}