using Data.Records.Types;

namespace Data;

public interface IRepository<TRecord> where TRecord : IDatabaseRecord
{
    Task<TRecord> SaveAsync(TRecord record);
    Task<List<TRecord>> SaveManyAsync(IEnumerable<TRecord> records);
    Task<TRecord> UpdateAsync(TRecord record);
    Task<List<TRecord>> UpdateManyAsync(IEnumerable<TRecord> records);
    Task<TRecord> DeleteAsync(TRecord record);
    Task<List<TRecord>> DeleteManyAsync(IEnumerable<TRecord> records);
}

public abstract class Repository<TRecord> : IRepository<TRecord> where TRecord : IDatabaseRecord
{
    protected IDatabase Database { get; }

    protected Repository(IDatabase database)
    {
        Database = database;
    }

    public async Task<TRecord> SaveAsync(TRecord record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.SaveAsync(record);

        await transaction.CommitAsync();

        return record;
    }

    public async Task<List<TRecord>> SaveManyAsync(IEnumerable<TRecord> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            await session.SaveAsync(record);

        await transaction.CommitAsync();

        return recordsAsList;
    }

    public async Task<TRecord> UpdateAsync(TRecord record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.UpdateAsync(record);

        await transaction.CommitAsync();

        return record;
    }

    public async Task<List<TRecord>> UpdateManyAsync(IEnumerable<TRecord> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            await session.UpdateAsync(record);

        await transaction.CommitAsync();

        return recordsAsList;
    }

    public async Task<TRecord> DeleteAsync(TRecord record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.DeleteAsync(record);

        await transaction.CommitAsync();

        return record;
    }

    public async Task<List<TRecord>> DeleteManyAsync(IEnumerable<TRecord> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            await session.DeleteAsync(record);

        await transaction.CommitAsync();

        return recordsAsList;
    }
}