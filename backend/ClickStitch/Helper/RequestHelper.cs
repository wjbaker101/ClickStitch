using CrossStitchViewer.Models;

namespace CrossStitchViewer.Helper;

public static class RequestHelper
{
    public static UserModel GetUser(HttpRequest httpRequest)
    {
        return httpRequest.HttpContext.Items["user"] as UserModel ?? throw new Exception("User expected on the request but was not found.");
    }
}