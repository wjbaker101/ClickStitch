﻿using Data.Records.Types;
using Data.Records.Values;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserPermissionRecord : IDatabaseRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required PermissionRecord Permission { get; init; }
}

public sealed class UserPermissionRecordMap : ClassMap<UserPermissionRecord>
{
    public UserPermissionRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("user_permission");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_permission_id_seq");
        References(x => x.User, "user_id");
        References(x => x.Permission, "permission_id");
    }
}