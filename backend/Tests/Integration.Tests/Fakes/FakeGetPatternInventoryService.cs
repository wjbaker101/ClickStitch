using ClickStitch.Api.Patterns.GetPatternInventory;
using ClickStitch.Api.Patterns.GetPatternInventory.Types;
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