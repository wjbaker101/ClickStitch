using ClickStitch.Api.Auth;
using ClickStitch.Api.Users.Types;
using ClickStitch.Models.Mappers;
using Core.Services;
using Core.Types;
using Data.Records;
using Data.Repositories.User;
using System.Text.RegularExpressions;

namespace ClickStitch.Api.Users;

public interface IUsersService
{
    Task<Result<GetSelfResponse>> GetSelf(RequestUser requestUser);
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request);
    Task<Result<UpdateUserResponse>> UpdateUser(RequestUser requestUser, Guid userReference, UpdateUserRequest request);
    Task<Result<DeleteUserResponse>> DeleteUser(RequestUser requestUser, Guid userReference);
}

public sealed partial class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IGuid _guid;
    private readonly IDateTime _dateTime;

    [GeneratedRegex(".+@.+\\..+")]
    private static partial Regex EmailRegex();

    public UsersService(IUserRepository userRepository, IPasswordService passwordService, IGuid guid, IDateTime dateTime)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _guid = guid;
        _dateTime = dateTime;
    }

    public async Task<Result<GetSelfResponse>> GetSelf(RequestUser requestUser)
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
        if (!EmailRegex().IsMatch(request.Email))
            return Result<CreateUserResponse>.Failure("Email is invalid, please try again.");

        var isValidResult = _passwordService.IsValid(request.Password);
        if (isValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(isValidResult);

        var byEmailResult = await _userRepository.GetByEmailAsync(request.Email);
        if (byEmailResult.IsSuccess)
            return Result<CreateUserResponse>.Failure("Cannot use that email, an existing user already has it. Please try again with a different email.");

        var passwordSalt = _guid.NewGuid().ToString();
        var password = _passwordService.Hash(request.Password, passwordSalt);

        var user = await _userRepository.SaveAsync(new UserRecord
        {
            Reference = _guid.NewGuid(),
            CreatedAt = _dateTime.UtcNow(),
            Email = request.Email,
            Password = password,
            PasswordSalt = passwordSalt
        });

        return new CreateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<UpdateUserResponse>> UpdateUser(RequestUser requestUser, Guid userReference, UpdateUserRequest request)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        await _userRepository.UpdateAsync(user);

        return new UpdateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<DeleteUserResponse>> DeleteUser(RequestUser requestUser, Guid userReference)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference);
        if (!userResult.TrySuccess(out var user))
            return Result<DeleteUserResponse>.FromFailure(userResult);

        await _userRepository.DeleteAsync(user);

        return new DeleteUserResponse();
    }
}