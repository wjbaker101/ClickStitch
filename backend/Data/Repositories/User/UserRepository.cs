using Core.Types;
using Data.Records;
using NHibernate.Linq;

// ReSharper disable SpecifyStringComparison

namespace Data.Repositories.User;

public interface IUserRepository : IRepository<UserRecord>
{
    Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference);
    Task<Result<UserRecord>> GetByUsernameAsync(string username);
    Task<Result<UserRecord>> GetByEmailAsync(string email);
}

public sealed class UserRepository : Repository<UserRecord>, IUserRepository
{
    public UserRepository(IDatabase database) : base(database)
    {
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
            .SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with username: '{username}'.");

        await transaction.CommitAsync();

        return user;
    }

    public async Task<Result<UserRecord>> GetByEmailAsync(string email)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with email: '{email}'.");

        await transaction.CommitAsync();

        return user;
    }
}