using Core.Types;

namespace TestHelpers.Models;

public sealed class TestRequestUser
{
    public long Id { get; set; } = 5734;
    public Guid Reference { get; set; } = Guid.Parse("81dd4bad-4c05-42f5-be8e-3e77425241ae");

    public static implicit operator RequestUser(TestRequestUser user)
    {
        return new RequestUser
        {
            Id = user.Id,
            Reference = user.Reference
        };
    }
}