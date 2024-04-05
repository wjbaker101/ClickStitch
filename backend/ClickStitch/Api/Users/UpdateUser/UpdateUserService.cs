using ClickStitch.Api.Users.UpdateUser.Types;
using Data.Repositories.User;

namespace ClickStitch.Api.Users.UpdateUser;

public interface IUpdateUserService
{
    Task<Result<UpdateUserResponse>> UpdateUser(RequestUser requestUser, Guid userReference, UpdateUserRequest request, CancellationToken cancellationToken);
}

public sealed class UpdateUserService : IUpdateUserService
{
    private readonly IUserRepository _userRepository;

    public UpdateUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UpdateUserResponse>> UpdateUser(RequestUser requestUser, Guid userReference, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        if (requestUser.Reference != user.Reference)
            return Result<UpdateUserResponse>.Failure("Cannot update a different user.");

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new UpdateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }
}