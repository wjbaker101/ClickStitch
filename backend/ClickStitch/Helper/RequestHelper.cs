namespace ClickStitch.Helper;

public static class RequestHelper
{
    public const string REQUEST_USER_ITEM_KEY = "user";

    public static RequestUser GetRequiredUser(HttpRequest httpRequest) => GetUser(httpRequest) ?? throw new Exception("User expected on the request but was not found.");
    
    public static RequestUser? GetOptionalUser(HttpRequest httpRequest) => GetUser(httpRequest);

    private static RequestUser? GetUser(HttpRequest httpRequest)
    {
        var isUserPresent = httpRequest.HttpContext.Items.TryGetValue(REQUEST_USER_ITEM_KEY, out var user);

        return isUserPresent ? user as RequestUser : null;
    }
}