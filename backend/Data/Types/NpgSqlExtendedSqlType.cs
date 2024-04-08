using NHibernate.SqlTypes;
using NpgsqlTypes;
using System.Data;

namespace Data.Types;

public sealed class NpgSqlExtendedSqlType : SqlType
{
    public NpgsqlDbType NpgDbType { get; }

    public NpgSqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType) : base(dbType)
    {
        NpgDbType = npgDbType;
    }

    public NpgSqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length) : base(dbType, length)
    {
        NpgDbType = npgDbType;
    }

    public NpgSqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale) : base(dbType, precision, scale)
    {
        NpgDbType = npgDbType;
    }
}