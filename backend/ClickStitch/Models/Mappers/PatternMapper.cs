﻿using Data.Records;

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
        ExternalShopUrl = pattern.ExternalShopUrl
    };

    public static StitchModel MapStitch(PatternStitchRecord stitch, UserPatternStitchRecord? userPatternStitch) => new()
    {
        ThreadIndex = stitch.ThreadIndex,
        X = stitch.X,
        Y = stitch.Y,
        StitchedAt = userPatternStitch?.StitchedAt
    };

    public static ThreadModel MapThread(PatternThreadRecord thread) => new()
    {
        Index = thread.Index,
        Name = thread.Name,
        Description = thread.Description,
        Colour = thread.Colour
    };
}