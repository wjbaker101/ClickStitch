using ClickStitch.Api.Auth;
using ClickStitch.Api.Users.Types;
using Core.Extensions;
using Core.Services;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.User;
using System.Text.RegularExpressions;

namespace ClickStitch.Api.Users;

public interface IUsersService
{
    Task<Result<GetSelfResponse>> GetSelf(RequestUser requestUser, CancellationToken cancellationToken);
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result<UpdateUserResponse>> UpdateUser(RequestUser requestUser, Guid userReference, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result<DeleteUserResponse>> DeleteUser(RequestUser requestUser, Guid userReference, CancellationToken cancellationToken);
    Task<Result<GetCreatorByUserResponse>> GetCreatorByUser(RequestUser requestUser, CancellationToken cancellationToken);
}

public sealed partial class UsersService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IGuid _guid;
    private readonly IDateTime _dateTime;
    private readonly ICreatorRepository _creatorRepository;

    [GeneratedRegex(".+@.+\\..+")]
    private static partial Regex EmailRegex();

    public UsersService(IUserRepository userRepository, IPasswordService passwordService, IGuid guid, IDateTime dateTime, ICreatorRepository creatorRepository)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _guid = guid;
        _dateTime = dateTime;
        _creatorRepository = creatorRepository;
    }

    public async Task<Result<GetSelfResponse>> GetSelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetWithPermissionsByReferenceAsync(requestUser.Reference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<GetSelfResponse>.FromFailure(userResult);

        return new GetSelfResponse
        {
            User = UserMapper.Map(user),
            Permissions = user.Permissions.MapAll(PermissionMapper.Map)
        };
    }

    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (!EmailRegex().IsMatch(request.Email))
            return Result<CreateUserResponse>.Failure("Email is invalid, please try again.");

        var isValidResult = _passwordService.IsValid(request.Password);
        if (isValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(isValidResult);

        var byEmailResult = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
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
            PasswordSalt = passwordSalt,
            LastLoginAt = null,
            Permissions = new List<PermissionRecord>()
        }, cancellationToken);

        return new CreateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<UpdateUserResponse>> UpdateUser(RequestUser requestUser, Guid userReference, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<UpdateUserResponse>.FromFailure(userResult);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new UpdateUserResponse
        {
            User = UserMapper.Map(user)
        };
    }

    public async Task<Result<DeleteUserResponse>> DeleteUser(RequestUser requestUser, Guid userReference, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByReferenceAsync(userReference, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<DeleteUserResponse>.FromFailure(userResult);

        await _userRepository.DeleteAsync(user, cancellationToken);

        return new DeleteUserResponse();
    }

    public async Task<Result<GetCreatorByUserResponse>> GetCreatorByUser(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var creatorResult = await _creatorRepository.GetByUser(user, cancellationToken);
        if (creatorResult.IsFailure)
        {
            return new GetCreatorByUserResponse
            {
                Creator = null
            };
        }

        return new GetCreatorByUserResponse
        {
            Creator = CreatorMapper.Map(creatorResult.Content)
        };
    }
}