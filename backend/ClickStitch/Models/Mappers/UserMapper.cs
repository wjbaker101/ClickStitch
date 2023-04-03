using Data.Records;

namespace ClickStitch.Models.Mappers;

public static class UserMapper
{
    public static UserModel Map(UserRecord user) => new()
    {
        Reference = user.Reference,
        CreatedAt = user.CreatedAt,
        Email = user.Email
    };
}