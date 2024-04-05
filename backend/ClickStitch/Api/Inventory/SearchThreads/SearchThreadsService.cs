using ClickStitch.Api.Inventory.SearchThreads.Types;
using Data.Records;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using Data.Repositories.UserThread.Types;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Inventory.SearchThreads;

public interface ISearchThreadsService
{
    Task<Result<SearchThreadsResponse>> SearchThreads(RequestUser requestUser, SearchThreadsParameters parameters, CancellationToken cancellationToken);
}

public sealed class SearchThreadsService : ISearchThreadsService
{
    private readonly IThreadRepository _threadRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserThreadRepository _userThreadRepository;

    public SearchThreadsService(IThreadRepository threadRepository, IUserRepository userRepository, IUserThreadRepository userThreadRepository)
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
}