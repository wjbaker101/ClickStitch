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
}