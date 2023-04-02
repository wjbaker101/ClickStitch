using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserPatternStitchRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required long UserPattern { get; init; }
    public virtual required PatternStitchRecord PatternStitch { get; init; }
    public virtual required DateTime? StitchedAt { get; set; }
}

public sealed class UserPatternStitchRecordMap : ClassMap<UserPatternStitchRecord>
{
    public UserPatternStitchRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("user_pattern_stitch");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_pattern_stitch_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.UserPattern, "user_pattern_id");
        References(x => x.PatternStitch, "pattern_stitch_id");
        Map(x => x.StitchedAt, "stitched_at");
    }
}