using ClickStitch.Api.Creators.SearchCreatorPatterns;
using ClickStitch.Api.Creators.SearchCreatorPatterns.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeSearchCreatorPatternsService : ISearchCreatorPatternsService
{
    public Task<Result<SearchCreatorPatternsResponse>> SearchCreatorPatterns(
        RequestUser user,
        Guid creatorReference,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<SearchCreatorPatternsResponse>>(new SearchCreatorPatternsResponse
        {
            Patterns = null!,
            ProjectPatternReferencesForUser = [],
            Pagination = null!
        });
    }
}