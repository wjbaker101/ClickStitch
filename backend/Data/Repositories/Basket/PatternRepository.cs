using Data.Records;
using NHibernate.Linq;

namespace Data.Repositories.Basket;

public interface IBasketRepository : IRepository<UserBasketItemRecord>
{
    List<UserBasketItemRecord> GetByUser(UserRecord user);
    Task<List<UserBasketItemRecord>> GetByUserAsync(UserRecord user);
}

public sealed class BasketRepository : Repository<UserBasketItemRecord>, IBasketRepository
{
    public BasketRepository(IDatabase database) : base(database)
    {
    }

    public List<UserBasketItemRecord> GetByUser(UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var basket = session
            .Query<UserBasketItemRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.User == user)
            .ToList();

        transaction.Commit();

        return basket;
    }

    public async Task<List<UserBasketItemRecord>> GetByUserAsync(UserRecord user)
    {
        using var session = Database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var basket = await session
            .Query<UserBasketItemRecord>()
            .Fetch(x => x.Pattern)
            .Where(x => x.User == user)
            .ToListAsync();

        await transaction.CommitAsync();

        return basket;
    }
}