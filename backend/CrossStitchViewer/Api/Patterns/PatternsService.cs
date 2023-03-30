using Core.Types;
using CrossStitchViewer.Api.Patterns.Types;
using CrossStitchViewer.Mappers;
using Data.Repositories.Pattern;
using Data.Repositories.Pattern.Types;

namespace CrossStitchViewer.Api.Patterns;

public interface IPatternsService
{
    Result<GetPatternsResponse> GetPatterns();
}

public sealed class PatternsService : IPatternsService
{
    private readonly IPatternRepository _patternRepository;

    public PatternsService(IPatternRepository patternRepository)
    {
        _patternRepository = patternRepository;
    }

    public Result<GetPatternsResponse> GetPatterns()
    {
        var patterns = _patternRepository.Search(new SearchPatternsParameters());

        return new GetPatternsResponse
        {
            Patterns = patterns.ConvertAll(PatternMapper.Map)
        };
    }
}