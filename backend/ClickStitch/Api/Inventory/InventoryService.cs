using ClickStitch.Api.Inventory.Types;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;

namespace ClickStitch.Api.Inventory;

public interface IInventoryService
{
    Task<Result<GetThreadsResponse>> GetThreads(RequestUser requestUser, CancellationToken cancellationToken);
    Task<Result<UpdateThreadResponse>> UpdateThread(RequestUser requestUser, Guid threadReference, UpdateThreadRequest request, CancellationToken cancellationToken);
}

public sealed class InventoryService : IInventoryService
{
    private readonly IThreadRepository _threadRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserThreadRepository _userThreadRepository;

    public InventoryService(IThreadRepository threadRepository, IUserRepository userRepository, IUserThreadRepository userThreadRepository)
    {
        _threadRepository = threadRepository;
        _userRepository = userRepository;
        _userThreadRepository = userThreadRepository;
    }

    public async Task<Result<GetThreadsResponse>> GetThreads(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var threads = await _threadRepository.GetAll(cancellationToken);

        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var userThreads = await _userThreadRepository.GetByUser(user, cancellationToken);
        var userThreadLookup = userThreads.ToDictionary(x => x.Thread.Id);

        var inventory = threads
            .Select(x =>
            {
                var hasUserThread = userThreadLookup.ContainsKey(x.Id);

                return new GetThreadsResponse.InventoryThread
                {
                    Thread = ThreadMapper.Map(x),
                    Count = hasUserThread ? userThreadLookup[x.Id].Count : 0
                };
            })
            .OrderBy(x => x.Thread.Code);

        return new GetThreadsResponse
        {
            Threads = inventory.ToList()
        };
    }

    public async Task<Result<UpdateThreadResponse>> UpdateThread(RequestUser requestUser, Guid threadReference, UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var threadResult = await _threadRepository.GetByReference(threadReference, cancellationToken);
        if (threadResult.IsFailure)
            return Result<UpdateThreadResponse>.FromFailure(threadResult);

        var userThreadResult = await _userThreadRepository.GetByUserAndThread(user, threadResult.Content, cancellationToken);
        if (!userThreadResult.TrySuccess(out var userThread))
            return Result<UpdateThreadResponse>.FromFailure(userThreadResult);

        userThread.Count = request.Count;

        await _userThreadRepository.SaveAsync(userThread, cancellationToken);

        return new UpdateThreadResponse();
    }
}