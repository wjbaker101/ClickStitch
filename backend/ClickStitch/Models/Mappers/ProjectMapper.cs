using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class ProjectMapper
{
    public static ProjectModel Map(UserPatternRecord userPattern) => new()
    {
        Pattern = PatternMapper.Map(userPattern.Pattern),
        PurchasedAt = userPattern.CreatedAt,
        LastPositionX = userPattern.LastPositionX,
        LastPositionY = userPattern.LastPositionY
    };
}