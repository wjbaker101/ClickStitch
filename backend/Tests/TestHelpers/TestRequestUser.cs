using Core.Types;

namespace TestHelpers;

public sealed class TestRequestUser
{
    public long Id { get; set; } = 6402;
    public Guid Reference { get; set; } = Guid.Parse("e95b2f73-71ce-4056-8b60-8c2b4411d871");
    public List<RequestPermissionType> Permissions { get; set; } = new();

    public static implicit operator RequestUser(TestRequestUser user) => new(user.Permissions)
    {
        Id = user.Id,
        Reference = user.Reference
    };
}