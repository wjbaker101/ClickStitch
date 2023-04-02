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
    private readonly ILoginTokenService _loginTokenService;

    public AuthService(IUserRepository userRepository, ILoginTokenService loginTokenService)
    {
        _userRepository = userRepository;
        _loginTokenService = loginTokenService;
    }

    public async Task<Result<LogInResponse>> LogIn(LogInRequest request)
    {
        var userResult = await _userRepository.GetByUsernameAsync(request.Username);
        if (userResult.IsFailure)
            return Result<LogInResponse>.FromFailure(userResult);

        var user = UserMapper.Map(userResult.Content);

        var loginToken = _loginTokenService.Create(user);

        return new LogInResponse
        {
            LoginToken = loginToken
        };
    }
}