using ClickStitch.Api.Projects.Types;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Projects.GetProjects;

public interface IGetProjectsService
{
    Task<Result<GetProjectsResponse>> GetProjects(RequestUser requestUser, CancellationToken cancellationToken);
}

public sealed class GetProjectsService : IGetProjectsService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;

    public GetProjectsService(IUserRepository userRepository, IUserPatternRepository userPatternRepository)
    {
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
    }

    public async Task<Result<GetProjectsResponse>> GetProjects(RequestUser requestUser, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

        var projects = await _userPatternRepository.GetByUserAsync(user, cancellationToken);

        return new GetProjectsResponse
        {
            Projects = projects.ConvertAll(ProjectMapper.Map)
        };
    }
}