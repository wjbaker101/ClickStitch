using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserPatternRecord : IDatabaseRecord
{
    public virtual required UserRecord User { get; init; }
    public virtual required PatternRecord Pattern { get; init; }
    public virtual required DateTime CreatedAt { get; init; }

    public override bool Equals(object? incoming)
    {
        if (incoming is not UserPatternRecord incomingRecord)
            return false;

        if (User.Id == incomingRecord.User.Id && Pattern.Id == incomingRecord.Pattern.Id)
            return true;

        return false;
    }

    public override int GetHashCode()
    {
        return User.Id.GetHashCode() + Pattern.Id.GetHashCode();
    }
}

public sealed class UserPatternRecordMap : ClassMap<UserPatternRecord>
{
    public UserPatternRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("user_pattern");
        CompositeId()
            .KeyReference(x => x.User, "user_id")
            .KeyReference(x => x.Pattern, "pattern_id");
        Map(x => x.CreatedAt, "created_at");
    }
}