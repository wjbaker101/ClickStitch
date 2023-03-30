using CrossStitchViewer.Models;

namespace TestHelpers.Models;

public static class TestUserModel
{
    public static UserModel Get() => new()
    {
        Reference = Guid.Parse("eb782ea5-0bca-45a7-bf6b-853e2943c426"),
        CreatedAt = new DateTime(2023, 05, 01, 16, 39, 14),
        Email = "test@email.com",
        Username = "TestUsername"
    };
}