﻿using NHibernate;
using NHibernate.Linq;
using NHibernate.Type;
using System.Collections;
using System.Linq.Expressions;

namespace TestHelpers.Data;

public sealed class TestQueryable<TRecord> : IOrderedQueryable<TRecord>
{
    public Type ElementType => _records.ElementType;
    public Expression Expression => _records.Expression;
    public IQueryProvider Provider => new TestNhQueryProvider<TRecord>(_records);

    private readonly IQueryable<TRecord> _records;

    public TestQueryable(IQueryable<TRecord> records)
    {
        _records = records;
    }

    public IEnumerator<TRecord> GetEnumerator()
    {
        return _records.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public sealed class TestNhQueryProvider<TRecord> : INhQueryProvider
{
    private readonly IQueryable<TRecord> _queryable;

    public TestNhQueryProvider(IQueryable<TRecord> queryable)
    {
        _queryable = queryable;
    }

    public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestQueryable<TElement>(_queryable.Provider.CreateQuery<TElement>(expression));
    }

    public object? Execute(Expression expression) => throw new NotImplementedException();

    public TResult Execute<TResult>(Expression expression)
    {
        return _queryable.Provider.Execute<TResult>(expression);
    }

    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        return Task.FromResult(Execute<TResult>(expression));
    }

    public int ExecuteDml<T>(QueryMode queryMode, Expression expression) => throw new NotImplementedException();

    public Task<int> ExecuteDmlAsync<T>(QueryMode queryMode, Expression expression, CancellationToken cancellationToken) => throw new NotImplementedException();

    public IFutureEnumerable<TResult> ExecuteFuture<TResult>(Expression expression) => throw new NotImplementedException();

    public IFutureValue<TResult> ExecuteFutureValue<TResult>(Expression expression) => throw new NotImplementedException();

    public void SetResultTransformerAndAdditionalCriteria(IQuery query, NhLinqExpression nhExpression, IDictionary<string, Tuple<object, IType>> parameters) => throw new NotImplementedException();
}