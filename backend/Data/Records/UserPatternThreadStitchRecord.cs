using Data.Types;

namespace Data.Records;

public class UserPatternThreadStitchRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required PatternThreadStitchRecord Stitch { get; init; }
    public virtual required DateTime StitchedAt { get; set; }
    public virtual required PatternThreadRecord Thread { get; set; }
    public virtual required int X { get; set; }
    public virtual required int Y { get; set; }
    public virtual required DateTime CompletedAt { get; set; }
}

public sealed class UserPatternThreadStitchRecordMap : ClassMap<UserPatternThreadStitchRecord>
{
    public UserPatternThreadStitchRecordMap()
    {
        Schema("clickstitch");
        Table("user_pattern_thread_stitch");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_pattern_thread_stitch_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Stitch, "pattern_thread_stitch_id");
        Map(x => x.StitchedAt, "stitched_at");
        References(x => x.Thread, "pattern_thread_id");
        Map(x => x.X, "x");
        Map(x => x.Y, "y");
        Map(x => x.CompletedAt, "completed_at");
    }
}