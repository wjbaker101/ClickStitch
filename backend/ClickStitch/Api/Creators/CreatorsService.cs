using ClickStitch.Api.Creators.Types;
using Data.Repositories.Creator;
using Data.Repositories.Creator.Types;
using Data.Repositories.User;
using Data.Repositories.UserCreator;

namespace ClickStitch.Api.Creators;

public interface ICreatorsService
{
    Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken);
}

public sealed class CreatorsService : ICreatorsService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserCreatorRepository _userCreatorRepository;

    public CreatorsService(
        ICreatorRepository creatorRepository,
        IUserRepository userRepository,
        IUserCreatorRepository userCreatorRepository)
    {
        _creatorRepository = creatorRepository;
        _userRepository = userRepository;
        _userCreatorRepository = userCreatorRepository;
    }

    public async Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var getPatternsResult = await _creatorRepository.GetCreatorPatterns(creatorReference, new GetCreatorPatternsParameters
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        }, cancellationToken);

        if (!getPatternsResult.TrySuccess(out var getPatterns))
            return Result<GetCreatorPatternsResponse>.FromFailure(getPatternsResult);

        return new GetCreatorPatternsResponse
        {
            Patterns = getPatterns.Patterns.ConvertAll(PatternMapper.Map),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getPatterns.TotalCount)
        };
    }
}