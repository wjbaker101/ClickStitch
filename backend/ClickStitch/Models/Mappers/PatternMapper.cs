using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class PatternMapper
{
    public static PatternModel MapWithCreator(PatternRecord pattern) => new()
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
        Creator = pattern.User.UserCreator == null ? null : new CreatorModel
        {
            Reference = pattern.User.UserCreator.Creator.Reference,
            CreatedAt = pattern.User.UserCreator.Creator.CreatedAt,
            Name = pattern.User.UserCreator.Creator.Name,
            StoreUrl = pattern.User.UserCreator.Creator.StoreUrl
        }
    };

    public static PatternModel MapWithoutCreator(PatternRecord pattern) => new()
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
        Creator = null
    };

    public static PatternThreadModel MapThread(PatternThreadRecord thread) => new()
    {
        Index = thread.Index,
        Name = thread.Name,
        Description = thread.Description,
        Colour = thread.Colour
    };
}