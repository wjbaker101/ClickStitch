namespace Data.Types;

public interface IRepository<TRecord> where TRecord : IDatabaseRecord
{
    Task<TRecord> SaveAsync(TRecord record, CancellationToken cancellationToken);
    Task<List<TRecord>> SaveManyAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken);
    Task<TRecord> UpdateAsync(TRecord record, CancellationToken cancellationToken);
    Task<List<TRecord>> UpdateManyAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken);
    Task<TRecord> DeleteAsync(TRecord record, CancellationToken cancellationToken);
    Task<List<TRecord>> DeleteManyAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken);
}

public abstract class Repository<TRecord> : IRepository<TRecord> where TRecord : IDatabaseRecord
{
    protected IDatabase Database { get; }

    protected Repository(IDatabase database)
    {
        Database = database;
    }

    public async Task<TRecord> SaveAsync(TRecord record, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.SaveAsync(record, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return record;
    }

    public async Task<List<TRecord>> SaveManyAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            await session.SaveAsync(record, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return recordsAsList;
    }

    public async Task<TRecord> UpdateAsync(TRecord record, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.UpdateAsync(record, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return record;
    }

    public async Task<List<TRecord>> UpdateManyAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            await session.UpdateAsync(record, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return recordsAsList;
    }

    public async Task<TRecord> DeleteAsync(TRecord record, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.DeleteAsync(record, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return record;
    }

    public async Task<List<TRecord>> DeleteManyAsync(IEnumerable<TRecord> records, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            await session.DeleteAsync(record, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return recordsAsList;
    }
}