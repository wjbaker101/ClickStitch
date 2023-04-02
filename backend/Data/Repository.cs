using Data.Records.Types;

namespace Data;

public interface IRepository<TRecord>  where TRecord : IDatabaseRecord
{
    TRecord Save(TRecord record);
    List<TRecord> SaveMany(IEnumerable<TRecord> records);
    TRecord Update(TRecord record);
    List<TRecord> UpdateMany(IEnumerable<TRecord> records);
    TRecord Delete(TRecord record);
    List<TRecord> DeleteMany(IEnumerable<TRecord> records);
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

    public TRecord Save(TRecord record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Save(record);

        transaction.Commit();

        return record;
    }

    public List<TRecord> SaveMany(IEnumerable<TRecord> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            session.Save(record);

        transaction.Commit();

        return recordsAsList;
    }

    public TRecord Update(TRecord record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Update(record);

        transaction.Commit();

        return record;
    }

    public List<TRecord> UpdateMany(IEnumerable<TRecord> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            session.Update(record);

        transaction.Commit();

        return recordsAsList;
    }

    public TRecord Delete(TRecord record)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        session.Delete(record);

        transaction.Commit();

        return record;
    }

    public List<TRecord> DeleteMany(IEnumerable<TRecord> records)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var recordsAsList = records.ToList();

        foreach (var record in recordsAsList)
            session.Delete(record);

        transaction.Commit();

        return recordsAsList;
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