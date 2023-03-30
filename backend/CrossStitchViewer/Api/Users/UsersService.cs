using Core.Services;
using Core.Types;
using CrossStitchViewer.Api.Auth;
using CrossStitchViewer.Api.Users.Types;
using CrossStitchViewer.Mappers;
using CrossStitchViewer.Models;
using Data.Records;
using Data.Repositories.User;

namespace CrossStitchViewer.Api.Users;

public interface IUsersService
{
    Result<GetSelfResponse> GetSelf(UserModel requestUser);
    Result<CreateUserResponse> CreateUser(CreateUserRequest request);
    Result<UpdateUserResponse> UpdateUser(UserModel requestUser, Guid userReference, UpdateUserRequest request);
    Result<DeleteUserResponse> DeleteUser(UserModel requestUser, Guid userReference);
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

    public Result<GetSelfResponse> GetSelf(UserModel requestUser)
    {
        var userResult = _userRepository.GetByReference(requestUser.Reference);
        if (!userResult.TrySuccess(out var user))
            return Result<GetSelfResponse>.FromFailure(userResult);

        return new GetSelfResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public Result<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var passwordSalt = _guidService.NewGuid();
        var password = _passwordService.Hash(request.Password, passwordSalt);

        var user = _userRepository.Save(new UserRecord
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

    public Result<UpdateUserResponse> UpdateUser(UserModel requestUser, Guid userReference, UpdateUserRequest request)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        user.Username = request.Username;

        _userRepository.Update(user);

        return new UpdateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public Result<DeleteUserResponse> DeleteUser(UserModel requestUser, Guid userReference)
    {
        var userResult = _userRepository.GetByReference(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<DeleteUserResponse>.FromFailure(userResult);

        _userRepository.Delete(user);

        return new DeleteUserResponse();
    }
}