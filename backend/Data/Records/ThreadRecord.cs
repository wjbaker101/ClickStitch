﻿using Data.Types;

namespace Data.Records;

public class ThreadRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required string Brand { get; set; }
    public virtual required string Code { get; set; }
    public virtual required string Colour { get; set; }
}

public sealed class ThreadRecordMap : ClassMap<ThreadRecord>
{
    public ThreadRecordMap()
    {
        Schema("clickstitch");
        Table("thread");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("thread_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.Brand, "brand");
        Map(x => x.Code, "code");
        Map(x => x.Colour, "colour");
    }
}