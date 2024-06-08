using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class CreatorMapper
{
    public static CreatorModel Map(CreatorRecord creator) => new()
    {
        Reference = creator.Reference,
        CreatedAt = creator.CreatedAt,
        Name = creator.Name,
        StoreUrl = creator.StoreUrl,
        Description = creator.Description ?? ""
    };
}