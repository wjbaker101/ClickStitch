using ClickStitch.Api.Creators.GetCreatorPatterns;
using ClickStitch.Api.Creators.GetCreatorPatterns.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeGetCreatorPatternsService : IGetCreatorPatternsService
{
    public Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(
        RequestUser user,
        Guid creatorReference,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<GetCreatorPatternsResponse>>(new GetCreatorPatternsResponse
        {
            Patterns = null!,
            Pagination = null!
        });
    }
}