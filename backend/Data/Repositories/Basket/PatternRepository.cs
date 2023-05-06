namespace Data.Repositories.Basket;

public interface IBasketRepository : IRepository<UserBasketItemRecord>
{
    Task<List<UserBasketItemRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken);
}

public sealed class BasketRepository : Repository<UserBasketItemRecord>, IBasketRepository
{
    public BasketRepository(IDatabase database) : base(database)
    {
    }

    public async Task<List<UserBasketItemRecord>> GetByUserAsync(UserRecord user, CancellationToken cancellationToken)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var basket = await session
            .Query<UserBasketItemRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.User == user)
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return basket;
    }
}