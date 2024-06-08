using ClickStitch.Api.Creators.GetCreator;
using ClickStitch.Api.Creators.GetCreator.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeGetCreatorService : IGetCreatorService
{
    public Task<Result<GetCreatorResponse>> GetCreator(Guid creatorReference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}