using ClickStitch.Api.Patterns.Types;
using Data.Records;
using Data.Repositories.Pattern;
using Data.Repositories.Pattern.Types;
using Data.Repositories.User;
using Data.Repositories.UserPattern;

namespace ClickStitch.Api.Patterns.SearchPatterns;

public interface ISearchPatternsService
{
    Task<Result<SearchPatternsResponse>> SearchPatterns(RequestUser? requestUser, CancellationToken cancellationToken);
}

public sealed class SearchPatternsService : ISearchPatternsService
{
    private readonly IPatternRepository _patternRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserPatternRepository _userPatternRepository;

    public SearchPatternsService(IPatternRepository patternRepository, IUserRepository userRepository, IUserPatternRepository userPatternRepository)
    {
        _patternRepository = patternRepository;
        _userRepository = userRepository;
        _userPatternRepository = userPatternRepository;
    }

    public async Task<Result<SearchPatternsResponse>> SearchPatterns(RequestUser? requestUser, CancellationToken cancellationToken)
    {
        var patternsToExclude = new List<PatternRecord>();

        if (requestUser != null)
        {
            var user = await _userRepository.GetByRequestUser(requestUser, cancellationToken);

            var projects = await _userPatternRepository.GetByUserAsync(user, cancellationToken);

            patternsToExclude.AddRange(projects.ConvertAll(x => x.Pattern));
        }

        var patterns = await _patternRepository.SearchAsync(new SearchPatternsParameters
        {
            PatternsToExclude = patternsToExclude
        }, cancellationToken);

        return new SearchPatternsResponse
        {
            Patterns = patterns.ConvertAll(PatternMapper.Map)
        };
    }
}