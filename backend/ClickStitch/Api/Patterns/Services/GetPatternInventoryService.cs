using ClickStitch.Api.Patterns.Types;
using Data.Repositories.Pattern;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;
using DotNetLibs.Core.Extensions;

namespace ClickStitch.Api.Patterns.Services;

public interface IGetPatternInventoryService
{
    Task<Result<GetPatternInventoryResponse>> GetPatternInventory(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class GetPatternInventoryService : IGetPatternInventoryService
{
    private readonly IUserRepository _userRepository;
    private readonly IPatternRepository _patternRepository;
    private readonly IThreadRepository _threadRepository;
    private readonly IUserThreadRepository _userThreadRepository;

    public GetPatternInventoryService(
        IUserRepository userRepository,
        IPatternRepository patternRepository,
        IThreadRepository threadRepository,
        IUserThreadRepository userThreadRepository)
    {
        _userRepository = userRepository;
        _patternRepository = patternRepository;
        _threadRepository = threadRepository;
        _userThreadRepository = userThreadRepository;
    }

    public async Task<Result<GetPatternInventoryResponse>> GetPatternInventory(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var patternResult = await _patternRepository.GetWithThreadsByReferenceAsync(patternReference, cancellationToken);
        if (!patternResult.TrySuccess(out var pattern))
            return Result<GetPatternInventoryResponse>.FromFailure(patternResult);

        var threads = await _threadRepository.GetByCodes(pattern.Threads.MapAll(x => GetCodeByThreadName(x.Name)), cancellationToken);

        var threadLookup = threads.ToDictionary(x => x.Id, x => x);

        var userThreads = await _userThreadRepository.GetByUserAndThreads(user, threadLookup.Keys.ToHashSet(), cancellationToken);

        return new GetPatternInventoryResponse
        {
            Threads = userThreads.ConvertAll(x => new GetPatternInventoryResponse.InventoryThread
            {
                Thread = ThreadMapper.Map(threadLookup[x.Thread.Id]),
                Count = x.Count
            })
        };
    }

    private static string GetCodeByThreadName(string name)
    {
        return name.Split(" ").Where(x => x.Length > 0).ToArray()[1].Trim();
    }
}