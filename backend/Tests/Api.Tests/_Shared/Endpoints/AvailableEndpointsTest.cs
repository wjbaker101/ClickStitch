using ClickStitch.Middleware.Authentication;
using ClickStitch.Middleware.Authorisation;
using DotNetLibs.Api.Types;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Api.Tests._Shared.Endpoints;

[TestFixture]
[Parallelizable]
public sealed class AvailableEndpointsTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        var controllers = typeof(Program).Assembly.GetTypes().Where(x => typeof(ApiController).IsAssignableFrom(x)).ToList();

        var controllersWithStuff = controllers.ConvertAll(controller => new
        {
            Controller = controller,
            Route = controller.CustomAttributes.Single(HasRoute).ConstructorArguments[0].Value,
            Methods = controller.GetMethods()
                .Where(method => method.CustomAttributes.Any(HasRoute))
                .Select(method => new
                {
                    Method = method,
                    HttpMethod = MapHttpMethod(method),
                    Route = method.CustomAttributes.Single(HasRoute).ConstructorArguments[0].Value,
                    Authenticates = method.CustomAttributes.Any(x => x.AttributeType == typeof(AuthenticateAttribute)),
                    RequiresAdmin = method.CustomAttributes.Any(x => x.AttributeType == typeof(RequireAdminAttribute)),
                    RequiresCreator = method.CustomAttributes.Any(x => x.AttributeType == typeof(RequireCreatorAttribute)),
                })
                .ToList()
        });

        var result = controllersWithStuff.SelectMany(controller => controller.Methods.ConvertAll(route => new
        {
            Method = route.HttpMethod,
            Route = $"{controller.Route}/{route.Route}",
            route.Authenticates,
            route.RequiresAdmin,
            route.RequiresCreator,
        }));

        Console.WriteLine("Done");
    }

    private static bool HasRoute(CustomAttributeData attribute)
    {
        return attribute.AttributeType == typeof(RouteAttribute);
    }

    private static string MapHttpMethod(MethodInfo method)
    {
        if (method.CustomAttributes.Any(x => x.AttributeType == typeof(HttpGetAttribute)))
            return "GET";
        if (method.CustomAttributes.Any(x => x.AttributeType == typeof(HttpPostAttribute)))
            return "POST";
        if (method.CustomAttributes.Any(x => x.AttributeType == typeof(HttpPutAttribute)))
            return "PUT";
        if (method.CustomAttributes.Any(x => x.AttributeType == typeof(HttpPatchAttribute)))
            return "PATCH";
        if (method.CustomAttributes.Any(x => x.AttributeType == typeof(HttpDeleteAttribute)))
            return "DELETE";

        return "UNKNOWN";
    }

    [Test]
    public void Then()
    {
    }
}