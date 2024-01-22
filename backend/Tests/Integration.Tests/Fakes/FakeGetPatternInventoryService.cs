using ClickStitch.Api.Patterns.Services;
using ClickStitch.Api.Patterns.Types;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeGetPatternInventoryService : IGetPatternInventoryService
{
    public Task<Result<GetPatternInventoryResponse>> GetPatternInventory(RequestUser requestUser, Guid patternReference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}