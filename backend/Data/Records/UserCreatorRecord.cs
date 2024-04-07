using Data.Types;

namespace Data.Records;

public class UserCreatorRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required CreatorRecord Creator { get; init; }
}

public sealed class UserCreatorRecordMap : ClassMap<UserCreatorRecord>
{
    public UserCreatorRecordMap()
    {
        Schema("clickstitch");
        Table("user_creator");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_creator_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Creator, "creator_id");
    }
}