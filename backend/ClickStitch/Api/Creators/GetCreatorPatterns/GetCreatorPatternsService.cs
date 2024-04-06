using ClickStitch.Api.Creators.GetCreatorPatterns.Types;
using Data.Repositories.Creator;
using Data.Repositories.Creator.Types;

namespace ClickStitch.Api.Creators.GetCreatorPatterns;

public interface IGetCreatorPatternsService
{
    Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(RequestUser user, Guid creatorReference, int pageSize, int pageNumber, CancellationToken cancellationToken);
}

public sealed class GetCreatorPatternsService : IGetCreatorPatternsService
{
    private readonly ICreatorRepository _creatorRepository;

    public GetCreatorPatternsService(ICreatorRepository creatorRepository)
    {
        _creatorRepository = creatorRepository;
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