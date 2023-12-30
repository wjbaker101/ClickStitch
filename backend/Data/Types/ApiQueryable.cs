using NHibernate;
using System.Linq.Expressions;

namespace Data.Types;

public interface IApiQueryable<TRecord> where TRecord : IDatabaseRecord
{
    IApiQueryable<TRecord> Where(Expression<Func<TRecord, bool>> predicate);
    IApiQueryable<TRecord> OrderByDescending<TKey>(Expression<Func<TRecord, TKey>> keySelector);
    IApiFetchQueryable<TRecord, TRelated> Fetch<TRelated>(Expression<Func<TRecord, TRelated>> relatedObjectSelector) where TRelated : IDatabaseRecord;
    IApiFetchQueryable<TRecord, TRelated> FetchMany<TRelated>(Expression<Func<TRecord, IEnumerable<TRelated>>> relatedObjectSelector) where TRelated : IDatabaseRecord;
    Task<TRecord> Single(CancellationToken cancellationToken);
    Task<TRecord> Single(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<TRecord?> SingleOrDefault(CancellationToken cancellationToken);
    Task<TRecord?> SingleOrDefault(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<List<TRecord>> ToList(CancellationToken cancellationToken);
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

    public IApiQueryable<TRecord> OrderByDescending<TKey>(Expression<Func<TRecord, TKey>> keySelector)
    {
        return new ApiQueryable<TRecord>(_queryable.OrderByDescending(keySelector));
    }

    public IApiFetchQueryable<TRecord, TRelated> Fetch<TRelated>(Expression<Func<TRecord, TRelated>> relatedObjectSelector) where TRelated : IDatabaseRecord
    {
        return new ApiFetchQueryable<TRecord, TRelated>(_queryable.Fetch(relatedObjectSelector));
    }

    public IApiFetchQueryable<TRecord, TRelated> FetchMany<TRelated>(Expression<Func<TRecord, IEnumerable<TRelated>>> relatedObjectSelector) where TRelated : IDatabaseRecord
    {
        return new ApiFetchQueryable<TRecord, TRelated>(_queryable.FetchMany(relatedObjectSelector));
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