using ClickStitch.Api.Creators.SearchCreatorPatterns.Types;
using Data.Records;
using Data.Repositories.Creator;
using Data.Repositories.Creator.Types;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Creators.SearchCreatorPatterns;

public interface ISearchCreatorPatternsService
{
    Task<Result<SearchCreatorPatternsResponse>> SearchCreatorPatterns(RequestUser requestUser, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken);
}

public sealed class SearchCreatorPatternsService : ISearchCreatorPatternsService
{
    private readonly ICreatorRepository _creatorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;

    public SearchCreatorPatternsService(ICreatorRepository creatorRepository, IUserRepository userRepository, IUserPatternRepository userPatternRepository)
    {
        _creatorRepository = creatorRepository;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
    }

    public async Task<Result<SearchCreatorPatternsResponse>> SearchCreatorPatterns(RequestUser? requestUser, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var getPatternsResult = await _creatorRepository.SearchCreatorPatterns(creatorReference, new GetCreatorPatternsParameters
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        }, cancellationToken);

        if (!getPatternsResult.TrySuccess(out var getPatterns))
            return Result<SearchCreatorPatternsResponse>.FromFailure(getPatternsResult);

        var userPatterns = new List<UserPatternRecord>();
        if (requestUser != null)
        {
            var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

            userPatterns = await _userPatternRepository.GetByUserAndPatternsAsync(user, getPatterns.Patterns, cancellationToken);
        }

        return new SearchCreatorPatternsResponse
        {
            Patterns = getPatterns.Patterns.ConvertAll(PatternMapper.Map),
            ProjectPatternReferencesForUser = userPatterns.ConvertAll(x => x.Pattern.Reference),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getPatterns.TotalCount)
        };
    }
}