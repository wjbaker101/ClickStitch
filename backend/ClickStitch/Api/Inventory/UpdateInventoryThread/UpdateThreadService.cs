using ClickStitch.Api.Inventory.UpdateInventoryThread.Types;
using Data.Records;
using Data.Repositories.Thread;
using Data.Repositories.User;
using Data.Repositories.UserThread;

namespace ClickStitch.Api.Inventory.UpdateInventoryThread;

public interface IUpdateInventoryThreadService
{
    Task<Result<UpdateInventoryThreadResponse>> UpdateInventoryThread(RequestUser requestUser, Guid threadReference, UpdateInventoryThreadRequest request, CancellationToken cancellationToken);
}

public sealed class UpdateInventoryThreadService : IUpdateInventoryThreadService
{
    private readonly IThreadRepository _threadRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserThreadRepository _userThreadRepository;

    public UpdateInventoryThreadService(IThreadRepository threadRepository, IUserRepository userRepository, IUserThreadRepository userThreadRepository)
    {
        _threadRepository = threadRepository;
        _userRepository = userRepository;
        _userThreadRepository = userThreadRepository;
    }

    public async Task<Result<UpdateInventoryThreadResponse>> UpdateInventoryThread(RequestUser requestUser, Guid threadReference, UpdateInventoryThreadRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var threadResult = await _threadRepository.GetByReference(threadReference, cancellationToken);
        if (threadResult.IsFailure)
            return Result<UpdateInventoryThreadResponse>.FromFailure(threadResult);

        var userThreadResult = await _userThreadRepository.GetByUserAndThread(user, threadResult.Content, cancellationToken);
        if (userThreadResult.TrySuccess(out var userThread))
        {
            if (request.Count == 0)
            {
                await _userThreadRepository.DeleteAsync(userThread, cancellationToken);

                return new UpdateInventoryThreadResponse();
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

        return new UpdateInventoryThreadResponse();
    }
}