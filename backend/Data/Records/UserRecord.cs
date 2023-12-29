using Data.Types;

namespace Data.Records;

public class UserRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required string Email { get; set; }
    public virtual required string Password { get; set; }
    public virtual required string PasswordSalt { get; set; }
    public virtual required DateTime? LastLoginAt { get; set; }
    public virtual required IList<PermissionRecord> Permissions { get; init; }
}

public sealed class UserRecordMap : ClassMap<UserRecord>
{
    public UserRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("user");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.Email, "email");
        Map(x => x.Password, "password");
        Map(x => x.PasswordSalt, "password_salt");
        Map(x => x.LastLoginAt, "last_login_at");
        HasManyToMany(x => x.Permissions)
            .Schema(DatabaseValues.SCHEMA)
            .Table("user_permission")
            .ParentKeyColumn("user_id")
            .ChildKeyColumn("permission_id");
    }
}