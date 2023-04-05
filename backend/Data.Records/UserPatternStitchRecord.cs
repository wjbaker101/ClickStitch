using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserPatternStitchRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserPatternRecord UserPattern { get; init; }
    public virtual required PatternStitchRecord Stitch { get; init; }
    public virtual required DateTime StitchedAt { get; set; }
    public virtual required int X { get; init; }
    public virtual required int Y { get; init; }
}

public sealed class UserPatternStitchRecordMap : ClassMap<UserPatternStitchRecord>
{
    public UserPatternStitchRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("user_pattern_stitch");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_pattern_stitch_id_seq");
        References(x => x.UserPattern, "user_pattern_id");
        References(x => x.Stitch, "pattern_stitch_id");
        Map(x => x.StitchedAt, "stitched_at");
        Map(x => x.X, "x");
        Map(x => x.Y, "y");
    }
}