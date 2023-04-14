using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class PermissionRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required PermissionType Type { get; init; }
    public virtual required string Name { get; init; }
}

public enum PermissionType
{
    Unknown = 0,
    Admin = 1
}

public sealed class PermissionRecordMap : ClassMap<PermissionRecord>
{
    public PermissionRecordMap()
    {
        Schema("cross_stitch_viewer");
        Table("permission");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("permission_id_seq");
        Map(x => x.Type, "type").CustomType<PermissionType>();
        Map(x => x.Name, "name");
    }
}