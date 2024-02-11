using Data.Types;

namespace Data.Records;

public class UserPatternRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required PatternRecord Pattern { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required int? PausePositionX { get; set; }
    public virtual required int? PausePositionY { get; set; }
}

public sealed class UserPatternRecordMap : ClassMap<UserPatternRecord>
{
    public UserPatternRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("user_pattern");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_pattern_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Pattern, "pattern_id");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.PausePositionX, "pause_position_x");
        Map(x => x.PausePositionY, "pause_position_y");
    }
}