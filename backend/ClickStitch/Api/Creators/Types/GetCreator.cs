namespace ClickStitch.Api.Creators.Types;

public sealed class GetCreatorResponse
{
    public required CreatorModel Creator { get; init; }
    public required List<UserModel> Users { get; init; }
}