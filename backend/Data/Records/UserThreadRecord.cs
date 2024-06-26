﻿using Data.Types;

namespace Data.Records;

public class UserThreadRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required ThreadRecord Thread { get; init; }
    public virtual required int Count { get; set; }
}

public sealed class UserThreadRecordMap : ClassMap<UserThreadRecord>
{
    public UserThreadRecordMap()
    {
        Schema("clickstitch");
        Table("user_thread");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_thread_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Thread, "thread_id");
        Map(x => x.Count, "count");
    }
}