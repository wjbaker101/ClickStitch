using ClickStitch.Api.Auth.Types;
using ClickStitch.Models.Mappers;
using Core.Types;
using Data.Repositories.User;

namespace ClickStitch.Api.Auth;

public interface IAuthService
{
    Task<Result<LogInResponse>> LogIn(LogInRequest request);
}

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ILoginTokenService _loginTokenService;

    public AuthService(IUserRepository userRepository, IPasswordService passwordService, ILoginTokenService loginTokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _loginTokenService = loginTokenService;
    }

    public async Task<Result<LogInResponse>> LogIn(LogInRequest request)
    {
        var userResult = await _userRepository.GetByUsernameAsync(request.Username);
        if (!userResult.TrySuccess(out var user))
            return Result<LogInResponse>.FromFailure(userResult);

        var isMatch = _passwordService.IsMatch(user.Password, request.Password, user.PasswordSalt);
        if (!isMatch)
            return Result<LogInResponse>.Failure("The given password was incorrect, please try again.");

        var loginToken = _loginTokenService.Create(UserMapper.Map(user));

        return new LogInResponse
        {
            LoginToken = loginToken
        };
    }
}