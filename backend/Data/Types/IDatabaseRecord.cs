namespace Data.Types;

public interface IDatabaseRecord
{
}

public interface IDatabaseRecordWithId : IDatabaseRecord
{
    long Id { get; init; }
}