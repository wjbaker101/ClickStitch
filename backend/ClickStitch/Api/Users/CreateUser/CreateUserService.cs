using ClickStitch.Api.Auth;
using ClickStitch.Api.Users.CreateUser.Types;
using Data.Records;
using Data.Repositories.User;
using DotNetLibs.Core.Services;
using System.Text.RegularExpressions;

namespace ClickStitch.Api.Users.CreateUser;

public interface ICreateUserService
{
    Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken);
}

public sealed partial class CreateUserService : ICreateUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    [GeneratedRegex(".+@.+\\..+")]
    private static partial Regex EmailRegex();

    public CreateUserService(IUserRepository userRepository, IPasswordService passwordService, IGuidProvider guidProvider, IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _guidProvider = guidProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();

        if (!EmailRegex().IsMatch(email))
            return Result<CreateUserResponse>.Failure("Email is invalid, please try again.");

        var isValidResult = _passwordService.IsValid(request.Password);
        if (isValidResult.IsFailure)
            return Result<CreateUserResponse>.FromFailure(isValidResult);

        var byEmailResult = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (byEmailResult.IsSuccess)
            return Result<CreateUserResponse>.Failure("Cannot use that email, an existing user already has it. Please try again with a different email.");

        var passwordSalt = _guidProvider.NewGuid().ToString();
        var password = _passwordService.Hash(request.Password, passwordSalt);

        var user = await _userRepository.SaveAsync(new UserRecord
        {
            Reference = _guidProvider.NewGuid(),
            CreatedAt = _dateTimeProvider.UtcNow(),
            Email = email,
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
}