using ClickStitch.Api.Creators.GetCreator.Types;
using Data.Repositories.Creator;

namespace ClickStitch.Api.Creators.GetCreator;

public interface IGetCreatorService
{
    Task<Result<GetCreatorResponse>> GetCreator(Guid creatorReference, CancellationToken cancellationToken);
}

public sealed class GetCreatorService : IGetCreatorService
{
    private readonly ICreatorRepository _creatorRepository;

    public GetCreatorService(ICreatorRepository creatorRepository)
    {
        _creatorRepository = creatorRepository;
    }

    public async Task<Result<GetCreatorResponse>> GetCreator(Guid creatorReference, CancellationToken cancellationToken)
    {
        var creatorResult = await _creatorRepository.GetByReference(creatorReference, cancellationToken);
        if (creatorResult.IsFailure)
            return Result<GetCreatorResponse>.FromFailure(creatorResult);

        return new GetCreatorResponse
        {
            Creator = CreatorMapper.Map(creatorResult.Content)
        };
    }
}