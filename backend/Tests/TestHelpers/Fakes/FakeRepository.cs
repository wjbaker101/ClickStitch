using Data;
using Data.Records.Types;

namespace TestHelpers.Fakes;

public abstract class FakeRepository<T> : IRepository<T> where T : IDatabaseRecord
{
    protected readonly T FakeValue;

    protected FakeRepository(T fakeValue)
    {
        FakeValue = fakeValue;
    }

    public Task<T> SaveAsync(T record, CancellationToken cancellationToken)
    {
        return Task.FromResult(record);
    }

    public Task<List<T>> SaveManyAsync(IEnumerable<T> records, CancellationToken cancellationToken)
    {
        return Task.FromResult(records.ToList());
    }

    public Task<T> UpdateAsync(T record, CancellationToken cancellationToken)
    {
        return Task.FromResult(record);
    }

    public Task<List<T>> UpdateManyAsync(IEnumerable<T> records, CancellationToken cancellationToken)
    {
        return Task.FromResult(records.ToList());
    }

    public Task<T> DeleteAsync(T record, CancellationToken cancellationToken)
    {
        return Task.FromResult(record);
    }

    public Task<List<T>> DeleteManyAsync(IEnumerable<T> records, CancellationToken cancellationToken)
    {
        return Task.FromResult(records.ToList());
    }
}