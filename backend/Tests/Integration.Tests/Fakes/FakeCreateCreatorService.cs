using ClickStitch.Api.Creators.CreateCreator;
using ClickStitch.Api.Creators.CreateCreator.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeCreateCreatorService : ICreateCreatorService
{
    public Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<CreateCreatorResponse>>(new CreateCreatorResponse
        {
            Creator = null!
        });
    }
}