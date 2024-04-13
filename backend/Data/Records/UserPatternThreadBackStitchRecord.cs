using Data.Types;

namespace Data.Records;

public class UserPatternThreadBackStitchRecord : IDatabaseRecord
{
    public long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required PatternThreadRecord Thread { get; init; }
    public virtual required int StartX { get; init; }
    public virtual required int StartY { get; init; }
    public virtual required int EndX { get; init; }
    public virtual required int EndY { get; init; }
    public virtual required DateTime CompletedAt { get; init; }
}

public sealed class UserPatternThreadBackStitchMap : ClassMap<UserPatternThreadBackStitchRecord>
{
    public UserPatternThreadBackStitchMap()
    {
        Schema("clickstitch");
        Table("user_pattern_thread_back_stitch");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_pattern_thread_back_stitch_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Thread, "pattern_thread_id");
        Map(x => x.StartX, "start_x");
        Map(x => x.StartY, "start_y");
        Map(x => x.EndX, "end_x");
        Map(x => x.EndY, "end_y");
        Map(x => x.CompletedAt, "completed_at");
    }
}