using Data.Types;

namespace Data.Records;

public class CreatorRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Name { get; set; }
    public virtual required string StoreUrl { get; set; }
    public virtual required string? Description { get; set; }
    public virtual required IList<UserRecord> Users { get; init; }
    public virtual required IList<PatternRecord> Patterns { get; init; }
}

public sealed class CreatorRecordMap : ClassMap<CreatorRecord>
{
    public CreatorRecordMap()
    {
        Schema("clickstitch");
        Table("creator");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("creator_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Name, "name");
        Map(x => x.StoreUrl, "store_url");
        Map(x => x.Description, "description");
        HasManyToMany(x => x.Users)
            .Table("user_creator")
            .Schema("clickstitch")
            .ParentKeyColumn("creator_id")
            .ChildKeyColumn("user_id");
        HasMany(x => x.Patterns)
            .KeyColumn("creator_id");
    }
}