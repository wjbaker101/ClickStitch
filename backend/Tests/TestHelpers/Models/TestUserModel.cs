using ClickStitch.Models;

namespace TestHelpers.Models;

public sealed class TestUserModel
{
    public Guid Reference { get; set; } = Guid.Parse("eb782ea5-0bca-45a7-bf6b-853e2943c426");
    public DateTime CreatedAt { get; set; } = new(2023, 05, 01, 16, 39, 14);
    public string Email { get; set; } = "test@email.com";

    public static implicit operator UserModel(TestUserModel user)
    {
        return new UserModel
        {
            Reference = user.Reference,
            CreatedAt = user.CreatedAt,
            Email = user.Email
        };
    }
}