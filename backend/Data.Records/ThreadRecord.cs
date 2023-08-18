namespace Data.Records;

public class ThreadRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required string Code { get; set; }
    public virtual required string Description { get; set; }
}

public sealed class ThreadRecordMap : ClassMap<ThreadRecord>
{
    public ThreadRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("thread");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("thread_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.Code, "code");
        Map(x => x.Description, "description");
    }
}