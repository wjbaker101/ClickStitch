using ClickStitch.Api.Creators.UpdateCreator;
using ClickStitch.Api.Creators.UpdateCreator.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeUpdateCreatorService : IUpdateCreatorService
{
    public Task<Result<UpdateCreatorResponse>> UpdateCreator(RequestUser requestUser, Guid creatorReference, UpdateCreatorRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<UpdateCreatorResponse>>(new UpdateCreatorResponse
        {
            Creator = null!
        });
    }
}