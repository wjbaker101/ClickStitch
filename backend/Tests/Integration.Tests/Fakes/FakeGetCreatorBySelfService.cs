using ClickStitch.Api.Creators.GetCreatorBySelf;
using ClickStitch.Api.Creators.GetCreatorBySelf.Types;
using ClickStitch.Models;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeGetCreatorBySelfService : IGetCreatorBySelfService
{
    public Task<Result<GetCreatorBySelfResponse>> GetCreatorBySelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<GetCreatorBySelfResponse>>(new GetCreatorBySelfResponse
        {
            Creator = new CreatorModel
            {
                Reference = Guid.Parse("8e13a53d-67b8-4329-ae3c-dfa64fe69dbf"),
                CreatedAt = default,
                Name = null!,
                StoreUrl = null!
            }
        });
    }
}