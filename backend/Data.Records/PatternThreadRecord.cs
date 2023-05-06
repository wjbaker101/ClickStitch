namespace Data.Records;

public class PatternThreadRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required PatternRecord Pattern { get; init; }
    public virtual required string Name { get; init; }
    public virtual required string Description { get; init; }
    public virtual required int Index { get; init; }
    public virtual required string Colour { get; init; }
}

public sealed class PatternThreadRecordMap : ClassMap<PatternThreadRecord>
{
    public PatternThreadRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("pattern_thread");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("pattern_thread_id_seq");
        References(x => x.Pattern, "pattern_id");
        Map(x => x.Name, "name");
        Map(x => x.Description, "description");
        Map(x => x.Index, "index");
        Map(x => x.Colour, "colour");
    }
}