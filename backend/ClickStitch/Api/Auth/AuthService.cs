using ClickStitch.Api.Auth.Types;
using Data.Repositories.User;
using Data.Repositories.UserPermission;
using DotNetLibs.Core.Services;

namespace ClickStitch.Api.Auth;

public interface IAuthService
{
    Task<Result<LogInResponse>> LogIn(LogInRequest request, CancellationToken cancellationToken);
}

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILoginTokenService _loginTokenService;
    private readonly IUserPermissionRepository _userPermissionRepository;
    private readonly IDateTimeProvider _dateTime;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ILoginTokenService loginTokenService,
        IUserPermissionRepository userPermissionRepository,
        IDateTimeProvider dateTime)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _loginTokenService = loginTokenService;
        _userPermissionRepository = userPermissionRepository;
        _dateTime = dateTime;
    }

    public async Task<Result<LogInResponse>> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        var isMatch = _passwordService.IsMatch(user.Password, request.Password, user.PasswordSalt);
        if (!isMatch)
            return Result<LogInResponse>.Failure("The given password was incorrect, please try again.");

        var loginToken = _loginTokenService.Create(UserMapper.Map(user));

        var permissions = await _userPermissionRepository.GetByUser(user, cancellationToken);

        user.LastLoginAt = _dateTime.UtcNow();

        await _userRepository.UpdateAsync(user, cancellationToken);

        return new LogInResponse
        {
            Reference = user.Reference,
            LoginToken = loginToken,
            Email = user.Email,
            Permissions = permissions.ConvertAll(PermissionMapper.Map)
        };
    }
}