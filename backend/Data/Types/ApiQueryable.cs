using System.Linq.Expressions;

namespace Data.Types;

public interface IApiQueryable<TRecord> where TRecord : IDatabaseRecord
{
    IApiQueryable<TRecord> FetchMany<TRelated>(Expression<Func<TRecord, IEnumerable<TRelated>>> relatedObjectSelector);
    Task<TRecord> Single(CancellationToken cancellationToken);
    Task<TRecord> Single(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<TRecord?> SingleOrDefault(CancellationToken cancellationToken);
    Task<TRecord?> SingleOrDefault(Expression<Func<TRecord, bool>> predicate, CancellationToken cancellationToken);
    Task<List<TRecord>> ToList(CancellationToken cancellationToken);
}

public sealed class ApiQueryable<TRecord> : IApiQueryable<TRecord> where TRecord : IDatabaseRecord
{
    private readonly IQueryable<TRecord> _queryable;

    public ApiQueryable(IQueryable<TRecord> queryable)
    {
        _queryable = queryable;
    }

    public IApiQueryable<TRecord> FetchMany<TRelated>(Expression<Func<TRecord, IEnumerable<TRelated>>> relatedObjectSelector)
    {
        return new ApiQueryable<TRecord>(_queryable.FetchMany(relatedObjectSelector));
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