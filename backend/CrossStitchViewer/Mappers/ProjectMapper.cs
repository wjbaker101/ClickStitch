﻿using CrossStitchViewer.Models;
using Data.Records;

namespace CrossStitchViewer.Mappers;

public static class ProjectMapper
{
    public static ProjectModel Map(UserPatternRecord userPattern) => new()
    {
        Pattern = PatternMapper.Map(userPattern.Pattern),
        PurchasedAt = userPattern.CreatedAt
    };
}