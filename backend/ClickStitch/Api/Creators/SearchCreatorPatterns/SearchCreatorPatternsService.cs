using ClickStitch.Api.Creators.SearchCreatorPatterns.Types;
using Data.Repositories.Creator;
using Data.Repositories.Creator.Types;

namespace ClickStitch.Api.Creators.SearchCreatorPatterns;

public interface ISearchCreatorPatternsService
{
    Task<Result<SearchCreatorPatternsResponse>> SearchCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken);
}

public sealed class SearchCreatorPatternsService : ISearchCreatorPatternsService
{
    private readonly ICreatorRepository _creatorRepository;

    public SearchCreatorPatternsService(ICreatorRepository creatorRepository)
    {
        _creatorRepository = creatorRepository;
    }

    public async Task<Result<SearchCreatorPatternsResponse>> SearchCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var getPatternsResult = await _creatorRepository.SearchCreatorPatterns(creatorReference, new GetCreatorPatternsParameters
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        }, cancellationToken);

        if (!getPatternsResult.TrySuccess(out var getPatterns))
            return Result<SearchCreatorPatternsResponse>.FromFailure(getPatternsResult);

        return new SearchCreatorPatternsResponse
        {
            Patterns = getPatterns.Patterns.ConvertAll(PatternMapper.Map),
            Pagination = PaginationModel.Create(pageNumber, pageSize, getPatterns.TotalCount)
        };
    }
}