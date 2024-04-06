using ClickStitch.Api.Patterns.GetPattern.Types;
using Data.Repositories.Pattern;

namespace ClickStitch.Api.Patterns.GetPattern;

public interface IGetPatternService
{
    Task<Result<GetPatternResponse>> GetPattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken);
}

public sealed class GetPatternService : IGetPatternService
{
    private readonly IPatternRepository _patternRepository;

    public GetPatternService(IPatternRepository patternRepository)
    {
        _patternRepository = patternRepository;

    }

    public async Task<Result<GetPatternResponse>> GetPattern(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        var patternResult = await _patternRepository.GetByReferenceAsync(patternReference, cancellationToken);
        if (patternResult.IsFailure)
            return Result<GetPatternResponse>.FromFailure(patternResult);

        return new GetPatternResponse
        {
            Pattern = PatternMapper.Map(patternResult.Content)
        };
    }
}