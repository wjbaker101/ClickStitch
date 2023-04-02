using ClickStitch.Api.Auth;
using ClickStitch.Api.Users.Types;
using ClickStitch.Models;
using ClickStitch.Models.Mappers;
using Core.Services;
using Core.Types;
using Data.Records;
using Data.Repositories.User;

namespace ClickStitch.Api.Users;

public interface IUsersService
{
    Task<Result<GetSelfResponse>> GetSelf(UserModel requestUser);
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request);
    Task<Result<UpdateUserResponse>> UpdateUser(UserModel requestUser, Guid userReference, UpdateUserRequest request);
    Task<Result<DeleteUserResponse>> DeleteUser(UserModel requestUser, Guid userReference);
}

public sealed class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IGuidService _guidService;
    private readonly IDateTimeService _dateTimeService;

    public UsersService(IUserRepository userRepository, IPasswordService passwordService, IGuidService guidService, IDateTimeService dateTimeService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _guidService = guidService;
        _dateTimeService = dateTimeService;
    }

    public async Task<Result<GetSelfResponse>> GetSelf(UserModel requestUser)
    {
        var userResult = await _userRepository.GetByReferenceAsync(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetSelfResponse>.FromFailure(userResult);

        return new GetSelfResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        var byEmailResult = await _userRepository.GetByEmailAsync(request.Email);
        if (byEmailResult.IsSuccess)
            return Result<CreateUserResponse>.Failure("Cannot use that email, an existing user already has it. Please try again with a different email.");

        var byUsernameResult = await _userRepository.GetByUsernameAsync(request.Username);
        if (byUsernameResult.IsSuccess)
            return Result<CreateUserResponse>.Failure("Cannot use that username, an existing user already has it. Please try again with a different username.");

        var passwordSalt = _guidService.NewGuid();
        var password = _passwordService.Hash(request.Password, passwordSalt);

        var user = await _userRepository.SaveAsync(new UserRecord
        {
            Reference = _guidService.NewGuid(),
            CreatedAt = _dateTimeService.UtcNow(),
            Email = request.Email,
            Username = request.Username,
            Password = password,
            PasswordSalt = passwordSalt.ToString()
        });

        return new CreateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<UpdateUserResponse>> UpdateUser(UserModel requestUser, Guid userReference, UpdateUserRequest request)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        user.Username = request.Username;

        await _userRepository.UpdateAsync(user);

        return new UpdateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<DeleteUserResponse>> DeleteUser(UserModel requestUser, Guid userReference)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<DeleteUserResponse>.FromFailure(userResult);

        await _userRepository.DeleteAsync(user);

        return new DeleteUserResponse();
    }
}