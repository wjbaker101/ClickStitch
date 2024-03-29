﻿using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class ProjectMapper
{
    public static ProjectModel Map(UserPatternRecord userPattern) => new()
    {
        Pattern = PatternMapper.Map(userPattern.Pattern),
        Reference = userPattern.Reference,
        PurchasedAt = userPattern.CreatedAt,
        PausePositionX = userPattern.PausePositionX,
        PausePositionY = userPattern.PausePositionY
    };
}