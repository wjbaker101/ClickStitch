using CrossStitchViewer.Models;
using Data.Records;

namespace CrossStitchViewer.Mappers;

public static class UserMapper
{
    public static UserModel Map(UserRecord user) => new()
    {
        Reference = user.Reference,
        CreatedAt = user.CreatedAt,
        Email = user.Email,
        Username = user.Username
    };
}