using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class PatternStitchRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required PatternRecord Pattern { get; init; }
    public virtual required int ThreadIndex { get; init; }
    public virtual required int X { get; init; }
    public virtual required int Y { get; init; }
}

public sealed class PatternStitchRecordMap : ClassMap<PatternStitchRecord>
{
    public PatternStitchRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("pattern_stitch");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("pattern_stitch_id_seq");
        References(x => x.Pattern, "pattern_id");
        Map(x => x.ThreadIndex, "thread_index");
        Map(x => x.X, "x");
        Map(x => x.Y, "y");
    }
}