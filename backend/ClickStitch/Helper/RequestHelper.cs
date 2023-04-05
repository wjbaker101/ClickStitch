namespace ClickStitch.Helper;

public sealed class RequestUser
{
    public required long Id { get; init; }
    public required Guid Reference { get; init; }
}

public static class RequestHelper
{
    public const string REQUEST_USER_ITEM_KEY = "user";

    public static RequestUser GetUser(HttpRequest httpRequest)
    {
        return httpRequest.HttpContext.Items[REQUEST_USER_ITEM_KEY] as RequestUser ?? throw new Exception("User expected on the request but was not found.");
    }
}