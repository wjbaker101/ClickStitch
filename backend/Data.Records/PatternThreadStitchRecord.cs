namespace Data.Records;

public class PatternThreadStitchRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required PatternThreadRecord Thread { get; init; }
    public virtual required int X { get; init; }
    public virtual required int Y { get; init; }
}

public sealed class PatternThreadStitchRecordMap : ClassMap<PatternThreadStitchRecord>
{
    public PatternThreadStitchRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("pattern_thread_stitch");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("pattern_thread_stitch_id_seq");
        References(x => x.Thread, "pattern_thread_id");
        Map(x => x.X, "x");
        Map(x => x.Y, "y");
    }
}