using Data.Repositories.Creator;

namespace ClickStitch.Api.Creators;

public interface ICreatorsService
{
}

public sealed class CreatorsService : ICreatorsService
{
    private readonly ICreatorRepository _creatorRepository;

    public CreatorsService(ICreatorRepository creatorRepository)
    {
        _creatorRepository = creatorRepository;
    }
}