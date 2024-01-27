using ClickStitch.Api.Inventory.Types;
using Data.Records;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Repositories.UserThread.Types;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Inventory;

public interface IInventoryService
{
    Task<Result<SearchThreadsResponse>> SearchThreads(RequestUser requestUser, SearchThreadsParameters parameters, CancellationToken cancellationToken);
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

    public async Task<Result<SearchThreadsResponse>> SearchThreads(RequestUser requestUser, SearchThreadsParameters parameters, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var userThreads = await _userThreadRepository.Search(user, new SearchUserThreadsParameters
        {
            SearchTerm = parameters.SearchTerm,
            Brand = parameters.Brand
        }, cancellationToken);

        var threads = new List<ThreadRecord>();
        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            threads = await _threadRepository.Search(new Data.Repositories.Thread.Types.SearchThreadsParameters
            {
                SearchTerm = parameters.SearchTerm,
                Brand = parameters.Brand
            }, cancellationToken);
        }

        var userThreadLookup = userThreads.Select(x => x.Thread.Id).ToHashSet();

        return new SearchThreadsResponse
        {
            InventoryThreads = userThreads.ConvertAll(x => new SearchThreadsResponse.InventoryThread
            {
                Thread = ThreadMapper.Map(x.Thread),
                Count = x.Count
            }),
            AvailableThreads = threads.Where(x => !userThreadLookup.Contains(x.Id)).MapAll(ThreadMapper.Map)
        };
    }

    public async Task<Result<UpdateThreadResponse>> UpdateThread(RequestUser requestUser, Guid threadReference, UpdateThreadRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var threadResult = await _threadRepository.GetByReference(threadReference, cancellationToken);
        if (threadResult.IsFailure)
            return Result<UpdateThreadResponse>.FromFailure(threadResult);

        var userThreadResult = await _userThreadRepository.GetByUserAndThread(user, threadResult.Content, cancellationToken);
        if (userThreadResult.TrySuccess(out var userThread))
        {
            if (request.Count == 0)
            {
                await _userThreadRepository.DeleteAsync(userThread, cancellationToken);

                return new UpdateThreadResponse();
            }

            userThread.Count = request.Count;

            await _userThreadRepository.UpdateAsync(userThread, cancellationToken);
        }
        else
        {
            await _userThreadRepository.SaveAsync(new UserThreadRecord
            {
                User = user,
                Thread = threadResult.Content,
                Count = request.Count
            }, cancellationToken);
        }

        return new UpdateThreadResponse();
    }
}