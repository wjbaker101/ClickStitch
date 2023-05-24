namespace ClickStitch.Models;

public sealed class UserModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Email { get; init; }
    public required DateTime? LastLoginAt { get; init; }
}