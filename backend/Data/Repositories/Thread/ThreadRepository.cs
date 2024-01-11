using Data.Repositories.Thread.Types;
using Data.Types;

namespace Data.Repositories.Thread;

public interface IThreadRepository : IRepository<ThreadRecord>
{
    Task<List<ThreadRecord>> Search(SearchThreadsParameters parameters, CancellationToken cancellationToken);
    Task<Result<ThreadRecord>> GetByReference(Guid threadReference, CancellationToken cancellationToken);
    Task<List<ThreadRecord>> GetAll(CancellationToken cancellationToken);
}

public sealed class ThreadRepository : Repository<ThreadRecord>, IThreadRepository
{
    public ThreadRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<ThreadRecord>> Search(SearchThreadsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var query = session
            .Query<ThreadRecord>();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            query = query.Where(x => x.Code.Contains(parameters.SearchTerm));

        if (!string.IsNullOrWhiteSpace(parameters.Brand))
            query = query.Where(x => x.Brand == parameters.Brand);

        var threads = await query
            .OrderBy(x => x.Code)
            .ToList(cancellationToken);

        await transaction.Commit(cancellationToken);

        return threads;
    }

    public async Task<Result<ThreadRecord>> GetByReference(Guid threadReference, CancellationToken cancellationToken)
    {
        using var session = Database.OpenSession();
        using var transaction = await session.BeginTransaction(cancellationToken);

        var thread = await session
            .Query<ThreadRecord>()
            .SingleOrDefault(x => x.Reference == threadReference, cancellationToken);

        await transaction.Commit(cancellationToken);

        if (thread == null)
            return Result<ThreadRecord>.Failure($"Unable to find thread with reference: '{threadReference}'.");

        return thread;
    }

    public async Task<List<ThreadRecord>> GetAll(CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var threads = await session
            .Query<ThreadRecord>()
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return threads;
    }
}