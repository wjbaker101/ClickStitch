namespace ClickStitch.Api.Users.Types;

public sealed class GetCreatorByUserResponse
{
    public required CreatorModel? Creator { get; init; }
}