using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Email { get; set; }
    public virtual required string Password { get; set; }
    public virtual required string PasswordSalt { get; set; }
    public virtual ISet<PatternRecord> Patterns { get; init; } = new HashSet<PatternRecord>();
}

public sealed class UserRecordMap : ClassMap<UserRecord>
{
    public UserRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("user");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Email, "email");
        Map(x => x.Password, "password");
        Map(x => x.PasswordSalt, "password_salt");
        HasManyToMany(x => x.Patterns)
            .Schema("cross_stitch_viewer")
            .Table("user_pattern")
            .ParentKeyColumn("user_id")
            .ChildKeyColumn("pattern_id")
            .Cascade.All();
    }
}