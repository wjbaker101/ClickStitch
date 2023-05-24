using ClickStitch.Api.Creators.Types;
using Core.Extensions;
using Data.Records;
using Data.Repositories.Creator;

namespace ClickStitch.Api.Creators;

public interface ICreatorsService
{
    Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken);
    Task<Result<GetCreatorResponse>> GetCreator(RequestUser requestUser, Guid creatorReference, CancellationToken cancellationToken);
}

public sealed class CreatorsService : ICreatorsService
{
    private readonly ICreatorRepository _creatorRepository;

    public CreatorsService(ICreatorRepository creatorRepository)
    {
        _creatorRepository = creatorRepository;
    }

    public async Task<Result<CreateCreatorResponse>> CreateCreator(RequestUser requestUser, CreateCreatorRequest request, CancellationToken cancellationToken)
    {
        var creator = await _creatorRepository.SaveAsync(new CreatorRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Name = request.Name,
            StoreUrl = request.StoreUrl,
            Users = new List<UserRecord>()
        }, cancellationToken);

        return new CreateCreatorResponse
        {
            Creator = CreatorMapper.Map(creator)
        };
    }

    public async Task<Result<GetCreatorResponse>> GetCreator(RequestUser requestUser, Guid creatorReference, CancellationToken cancellationToken)
    {
        var creatorResult = await _creatorRepository.GetWithUsersByReference(creatorReference, cancellationToken);
        if (creatorResult.IsFailure)
            return Result<GetCreatorResponse>.FromFailure(creatorResult);

        return new GetCreatorResponse
        {
            Creator = CreatorMapper.Map(creatorResult.Content),
            Users = creatorResult.Content.Users.MapAll(UserMapper.Map)
        };
    }
}