using ClickStitch.Api.Creators;
using ClickStitch.Api.Creators.Types;
using ClickStitch.Models;
using Core.Types;
using DotNetLibs.Core.Types;

namespace Integration.Tests.Fakes;

public sealed class FakeCreatorsService : ICreatorsService
{
    public Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UpdateCreatorResponse>> UpdateCreator(RequestUser requestUser, Guid creatorReference, UpdateCreatorRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<GetCreatorResponse>> GetCreator(RequestUser requestUser, Guid creatorReference, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<GetCreatorByUserResponse>> GetCreatorBySelf(RequestUser requestUser, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<GetCreatorByUserResponse>>(new GetCreatorByUserResponse
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

    public Task<Result<GetCreatorPatternsResponse>> GetCreatorPatterns(
        RequestUser user,
        Guid creatorReference,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}