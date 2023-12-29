using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class PatternMapper
{
    public static PatternModel Map(PatternRecord pattern) => new()
    {
        Reference = pattern.Reference,
        CreatedAt = pattern.CreatedAt,
        Title = pattern.Title,
        Width = pattern.Width,
        Height = pattern.Height,
        Price = pattern.Price,
        ThumbnailUrl = pattern.ThumbnailUrl,
        ThreadCount = pattern.ThreadCount,
        StitchCount = pattern.StitchCount,
        BannerImageUrl = pattern.BannerImageUrl,
        ExternalShopUrl = pattern.ExternalShopUrl,
        TitleSlug = pattern.TitleSlug,
        Creator = MapCreator(pattern.Creator)
    };

    private static CreatorModel? MapCreator(CreatorRecord? creator)
    {
        if (creator == null)
            return null;

        return new CreatorModel
        {
            Reference = creator.Reference,
            CreatedAt = creator.CreatedAt,
            Name = creator.Name,
            StoreUrl = creator.StoreUrl
        };
    }

    public static PatternThreadModel MapThread(PatternThreadRecord thread) => new()
    {
        Index = thread.Index,
        Name = thread.Name,
        Description = thread.Description,
        Colour = thread.Colour
    };
}