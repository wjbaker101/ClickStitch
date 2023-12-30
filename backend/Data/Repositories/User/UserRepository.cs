// ReSharper disable SpecifyStringComparison

using Core.Types;
using Data.Types;

namespace Data.Repositories.User;

public interface IUserRepository : IRepository<UserRecord>
{
    Task<UserRecord> GetByRequestUser(RequestUser requestUser, CancellationToken cancellationToken);
    Task<Result<UserRecord>> GetWithPermissionsByReferenceAsync(Guid userReference, CancellationToken cancellationToken);
    Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference, CancellationToken cancellationToken);
    Task<Result<UserRecord>> GetByEmailAsync(string email, CancellationToken cancellationToken);
}

public sealed class UserRepository : Repository<UserRecord>, IUserRepository
{
    public UserRepository(IDatabase database) : base(database)
    {
    }

    public async Task<UserRecord> GetByRequestUser(RequestUser requestUser, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();

        return await session.LoadAsync<UserRecord>(requestUser.Id, cancellationToken);
    }

    public async Task<Result<UserRecord>> GetWithPermissionsByReferenceAsync(Guid userReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .FetchMany(x => x.Permissions)
            .SingleOrDefaultAsync(x => x.Reference == userReference, cancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with reference: '{userReference}'.");

        await transaction.CommitAsync(cancellationToken);

        return user;
    }

    public async Task<Result<UserRecord>> GetByReferenceAsync(Guid userReference, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Reference == userReference, cancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with reference: '{userReference}'.");

        await transaction.CommitAsync(cancellationToken);

        return user;
    }

    public async Task<Result<UserRecord>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var user = await session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with email: '{email}'.");

        await transaction.Commit(cancellationToken);

        return user;
    }
}