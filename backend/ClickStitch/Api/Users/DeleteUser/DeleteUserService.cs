using ClickStitch.Api.Users.DeleteUser.Types;
using Data.Repositories.User;

namespace ClickStitch.Api.Users.DeleteUser;

public interface IDeleteUserService
{
    Task<Result<DeleteUserResponse>> DeleteUser(RequestUser requestUser, Guid userReference, CancellationToken cancellationToken);
}

public sealed class DeleteUserService : IDeleteUserService
{
    private readonly IUserRepository _userRepository;

    public DeleteUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<DeleteUserResponse>> DeleteUser(RequestUser requestUser, Guid userReference, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<DeleteUserResponse>.FromFailure(userResult);

        if (requestUser.Reference != user.Reference)
            return Result<DeleteUserResponse>.Failure("Cannot delete a different user.");

        await _userRepository.DeleteAsync(user, cancellationToken);

        return new DeleteUserResponse();
    }
}