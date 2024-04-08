using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.Common;
using Utf8Json;

namespace Data.Types;

[Serializable]
public sealed class JsonBlob<T> : IUserType where T : class
{
    public Type ReturnedType => typeof(T);
    public bool IsMutable => true;
    public SqlType[] SqlTypes => [ new NpgSqlExtendedSqlType(DbType.Object, NpgsqlDbType.Json) ];

    public new bool Equals(object? x, object? y)
    {
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;

        var jsonX = JsonSerializer.Serialize(x);
        var jsonY = JsonSerializer.Serialize(y);

        return jsonX == jsonY;
    }

    public int GetHashCode(object? x)
    {
        return x == null ? 0 : x.GetHashCode();
    }

    public object? NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        if (names.Length > 1)
            throw new InvalidOperationException("Only expected 1 column.");

        if (rs[names[0]] is string value && !string.IsNullOrWhiteSpace(value))
            return JsonSerializer.Deserialize<T>(value);

        return null;
    }

    public void NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session)
    {
        var parameter = (NpgsqlParameter)cmd.Parameters[index];
        parameter.NpgsqlDbType = NpgsqlDbType.Json;

        if (value == null)
            parameter.Value = DBNull.Value;
        else
            parameter.Value = JsonSerializer.Serialize(value);
    }

    public object? DeepCopy(object? value)
    {
        if (value == null)
            return null;

        var json = JsonSerializer.Serialize(value);

        return JsonSerializer.Deserialize<T>(json);
    }

    public object Replace(object original, object target, object owner)
    {
        return original;
    }

    public object? Assemble(object cached, object owner)
    {
        if (cached is string json && !string.IsNullOrWhiteSpace(json))
            return JsonSerializer.Deserialize<T>(json);

        return null;
    }

    public object? Disassemble(object? value)
    {
        return value == null ? null : JsonSerializer.Serialize(value);
    }
}