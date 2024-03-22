using NHibernate;
using NHibernate.Transform;
using NHibernate.Type;
using System.Collections;

namespace TestHelpers.Data;

public sealed class TestSqlQuery : ISQLQuery
{
    public string QueryString { get; }
    public IType[] ReturnTypes { get; }
    public string[] ReturnAliases { get; }
    public string[] NamedParameters { get; }
    public bool IsReadOnly { get; }

    private readonly string _sql;

    public TestSqlQuery(string sql)
    {
        _sql = sql;
    }

    public Task<IEnumerable> EnumerableAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> EnumerableAsync<T>(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<IList> ListAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task ListAsync(IList results, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<IList<T>> ListAsync<T>(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<object> UniqueResultAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<T> UniqueResultAsync<T>(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<int> ExecuteUpdateAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(0);
    }

    public IEnumerable Enumerable()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Enumerable<T>()
    {
        throw new NotImplementedException();
    }

    public IList List()
    {
        throw new NotImplementedException();
    }

    public void List(IList results)
    {
        throw new NotImplementedException();
    }

    public IList<T> List<T>()
    {
        throw new NotImplementedException();
    }

    public object UniqueResult()
    {
        throw new NotImplementedException();
    }

    public T UniqueResult<T>()
    {
        throw new NotImplementedException();
    }

    public int ExecuteUpdate()
    {
        throw new NotImplementedException();
    }

    public IQuery SetMaxResults(int maxResults)
    {
        throw new NotImplementedException();
    }

    public IQuery SetFirstResult(int firstResult)
    {
        throw new NotImplementedException();
    }

    public IQuery SetReadOnly(bool readOnly)
    {
        throw new NotImplementedException();
    }

    public IQuery SetCacheable(bool cacheable)
    {
        throw new NotImplementedException();
    }

    public IQuery SetCacheRegion(string cacheRegion)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimeout(int timeout)
    {
        throw new NotImplementedException();
    }

    public IQuery SetFetchSize(int fetchSize)
    {
        throw new NotImplementedException();
    }

    public IQuery SetLockMode(string alias, LockMode lockMode)
    {
        throw new NotImplementedException();
    }

    public IQuery SetComment(string comment)
    {
        throw new NotImplementedException();
    }

    public IQuery SetFlushMode(FlushMode flushMode)
    {
        throw new NotImplementedException();
    }

    public IQuery SetCacheMode(CacheMode cacheMode)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameter(int position, object val, IType type)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameter(string name, object val, IType type)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameter<T>(int position, T val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameter<T>(string name, T val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameter(int position, object val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameter(string name, object val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameterList(string name, IEnumerable vals, IType type)
    {
        throw new NotImplementedException();
    }

    public IQuery SetParameterList(string name, IEnumerable vals)
    {
        throw new NotImplementedException();
    }

    public IQuery SetProperties(object obj)
    {
        throw new NotImplementedException();
    }

    public IQuery SetAnsiString(int position, string val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetAnsiString(string name, string val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetBinary(int position, byte[] val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetBinary(string name, byte[] val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetBoolean(int position, bool val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetBoolean(string name, bool val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetByte(int position, byte val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetByte(string name, byte val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetCharacter(int position, char val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetCharacter(string name, char val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTime(int position, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTime(string name, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTimeNoMs(int position, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTimeNoMs(string name, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTime2(int position, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTime2(string name, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimeSpan(int position, TimeSpan val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimeSpan(string name, TimeSpan val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimeAsTimeSpan(int position, TimeSpan val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimeAsTimeSpan(string name, TimeSpan val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTimeOffset(int position, DateTimeOffset val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDateTimeOffset(string name, DateTimeOffset val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDecimal(int position, decimal val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDecimal(string name, decimal val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDouble(int position, double val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetDouble(string name, double val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetEnum(int position, Enum val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetEnum(string name, Enum val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetInt16(int position, short val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetInt16(string name, short val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetInt32(int position, int val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetInt32(string name, int val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetInt64(int position, long val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetInt64(string name, long val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetSingle(int position, float val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetSingle(string name, float val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetString(int position, string val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetString(string name, string val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTime(int position, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTime(string name, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimestamp(int position, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetTimestamp(string name, DateTime val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetGuid(int position, Guid val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetGuid(string name, Guid val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetEntity(int position, object val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetEntity(string name, object val)
    {
        throw new NotImplementedException();
    }

    public IQuery SetResultTransformer(IResultTransformer resultTransformer)
    {
        throw new NotImplementedException();
    }

    public IFutureEnumerable<T> Future<T>()
    {
        throw new NotImplementedException();
    }

    public IFutureValue<T> FutureValue<T>()
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddEntity(string entityName)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddEntity(string alias, string entityName)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddEntity(string alias, string entityName, LockMode lockMode)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddEntity(Type entityClass)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddEntity(string alias, Type entityClass)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddEntity(string alias, Type entityClass, LockMode lockMode)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddJoin(string alias, string path)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddJoin(string alias, string path, LockMode lockMode)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery AddScalar(string columnAlias, IType type)
    {
        throw new NotImplementedException();
    }

    public ISQLQuery SetResultSetMapping(string name)
    {
        throw new NotImplementedException();
    }
}