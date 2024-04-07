using NHibernate;
using System.Linq.Expressions;

namespace Data.Types;

public interface IApiQueryable<TRecord> where TRecord : IDatabaseRecord
{
    IApiQueryable<TRecord> Where(Expression<Func<TRecord, bool>> predicate);
    IApiQueryable<TOutput> Select<TOutput>(Expression<Func<TRecord, TOutput>> selector) where TOutput : IDatabaseRecord;
    IApiQueryable<TRecord> OrderBy<TKey>(Expression<Func<TRecord, TKey>> keySelector);
    IApiQueryable<TRecord> OrderByDescending<TKey>(Expression<Func<TRecord, TKey>> keySelector);
    IApiFetchQueryable<TRecord, TRelated> Fetch<TRelated>(Expression<Func<TRecord, TRelated>> relatedObjectSelector) where TRelated : IDatabaseRecord?;
    IApiFetchQueryable<TRecord, TRelated> FetchMany<TRelated>(Expression<Func<TRecord, IEnumerable<TRelated>>> relatedObjectSelector) where TRelated : IDatabaseRecord;
    IApiQueryable<TRecord> Skip(int count);
    IApiQueryable<TRecord> Take(int count);
    Task<TRecord> Single(CancellationToken cancellationToken);
    Task<TRecord> Single(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<TRecord?> SingleOrDefault(CancellationToken cancellationToken);
    Task<TRecord?> SingleOrDefault(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<List<TRecord>> ToList(CancellationToken cancellationToken);
    Task<bool> Any(CancellationToken cancellationToken);
    Task<bool> Any(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<int> Delete(CancellationToken cancellationToken);
    IFutureEnumerable<TRecord> ToFuture();
    IFutureValue<TResult> ToFutureValue<TResult>(Expression<Func<IQueryable<TRecord>, TResult>> selector);
}

public class ApiQueryable<TRecord> : IApiQueryable<TRecord> where TRecord : IDatabaseRecord
{
    private readonly IQueryable<TRecord> _queryable;

    public ApiQueryable(IQueryable<TRecord> queryable)
    {
        _queryable = queryable;
    }

    public IApiQueryable<TRecord> Where(Expression<Func<TRecord, bool>> predicate)
    {
        return new ApiQueryable<TRecord>(_queryable.Where(predicate));
    }

    public IApiQueryable<TOutput> Select<TOutput>(Expression<Func<TRecord, TOutput>> selector) where TOutput : IDatabaseRecord
    {
        return new ApiQueryable<TOutput>(_queryable.Select(selector));
    }

    public IApiQueryable<TRecord> OrderBy<TKey>(Expression<Func<TRecord, TKey>> keySelector)
    {
        return new ApiQueryable<TRecord>(_queryable.OrderBy(keySelector));
    }

    public IApiQueryable<TRecord> OrderByDescending<TKey>(Expression<Func<TRecord, TKey>> keySelector)
    {
        return new ApiQueryable<TRecord>(_queryable.OrderByDescending(keySelector));
    }

    public IApiFetchQueryable<TRecord, TRelated> Fetch<TRelated>(Expression<Func<TRecord, TRelated>> relatedObjectSelector) where TRelated : IDatabaseRecord?
    {
        return new ApiFetchQueryable<TRecord, TRelated>(_queryable.Fetch(relatedObjectSelector));
    }

    public IApiFetchQueryable<TRecord, TRelated> FetchMany<TRelated>(Expression<Func<TRecord, IEnumerable<TRelated>>> relatedObjectSelector) where TRelated : IDatabaseRecord
    {
        return new ApiFetchQueryable<TRecord, TRelated>(_queryable.FetchMany(relatedObjectSelector));
    }

    public IApiQueryable<TRecord> Skip(int count)
    {
        return new ApiQueryable<TRecord>(_queryable.Skip(count));
    }

    public IApiQueryable<TRecord> Take(int count)
    {
        return new ApiQueryable<TRecord>(_queryable.Take(count));
    }

    public async Task<TRecord> Single(CancellationToken cancellationToken)
    {
        return await _queryable.SingleAsync(cancellationToken);
    }

    public async Task<TRecord> Single(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _queryable.SingleAsync(predicate, cancellationToken);
    }

    public async Task<TRecord?> SingleOrDefault(CancellationToken cancellationToken)
    {
        return await _queryable.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TRecord?> SingleOrDefault(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _queryable.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<List<TRecord>> ToList(CancellationToken cancellationToken)
    {
        return await _queryable.ToListAsync(cancellationToken);
    }

    public async Task<bool> Any(CancellationToken cancellationToken)
    {
        return await _queryable.AnyAsync(cancellationToken);
    }

    public async Task<bool> Any(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _queryable.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> Delete(CancellationToken cancellationToken)
    {
        return await _queryable.DeleteAsync(cancellationToken);
    }

    public IFutureEnumerable<TRecord> ToFuture()
    {
        return _queryable.ToFuture();
    }

    public IFutureValue<TResult> ToFutureValue<TResult>(Expression<Func<IQueryable<TRecord>, TResult>> selector)
    {
        return _queryable.ToFutureValue(selector);
    }
}

public interface IApiFetchQueryable<TRecord, TFetch> : IApiQueryable<TRecord> where TRecord : IDatabaseRecord
{
    IApiFetchQueryable<TRecord, TRelated> ThenFetch<TRelated>(Expression<Func<TFetch, TRelated>> relatedObjectSelector) where TRelated : IDatabaseRecord;
}

public sealed class ApiFetchQueryable<TRecord, TFetch> : ApiQueryable<TRecord>, IApiFetchQueryable<TRecord, TFetch> where TRecord : IDatabaseRecord
{
    private readonly INhFetchRequest<TRecord, TFetch> _request;

    public ApiFetchQueryable(INhFetchRequest<TRecord, TFetch> request) : base(request)
    {
        _request = request;
    }

    public IApiFetchQueryable<TRecord, TRelated> ThenFetch<TRelated>(Expression<Func<TFetch, TRelated>> relatedObjectSelector) where TRelated : IDatabaseRecord
    {
        return new ApiFetchQueryable<TRecord, TRelated>(_request.ThenFetch(relatedObjectSelector));
    }
}