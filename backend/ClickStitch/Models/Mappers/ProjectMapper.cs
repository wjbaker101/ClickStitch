using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class ProjectMapper
{
    public static ProjectModel Map(UserPatternRecord userPattern) => new()
    {
        Pattern = PatternMapper.MapWithCreator(userPattern.Pattern),
        PurchasedAt = userPattern.CreatedAt
    };
}