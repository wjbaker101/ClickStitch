using ClickStitch.Api.Inventory.Types;
using Data.Repositories.Thread;

namespace ClickStitch.Api.Inventory;

public interface IInventoryService
{
    Task<Result<GetThreadsResponse>> GetThreads(RequestUser requestUser, CancellationToken cancellationToken);
}

public sealed class InventoryService : IInventoryService
{
    private readonly IThreadRepository _threadRepository;

    public InventoryService(IThreadRepository threadRepository)
    {
        _threadRepository = threadRepository;
    }

    public async Task<Result<GetThreadsResponse>> GetThreads(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var threads = await _threadRepository.GetAll(cancellationToken);

        return new GetThreadsResponse
        {
            Threads = threads.ConvertAll(ThreadMapper.Map)
        };
    }
}