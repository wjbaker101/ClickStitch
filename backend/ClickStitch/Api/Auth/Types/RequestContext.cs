namespace ClickStitch.Api.Auth.Types;

public sealed class RequestContext
{
    public required UserModel User { get; init; }
}