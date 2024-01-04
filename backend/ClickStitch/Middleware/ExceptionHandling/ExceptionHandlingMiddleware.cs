using DotNetLibs.Api.Types;
using System.Net;

namespace ClickStitch.Middleware.ExceptionHandling;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ApiErrorResponse("Apologies, something went wrong. Please refresh and try again later."));
        }
    }
}