using ClickStitch.Types;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ClickStitch.Filters;

public sealed class HttpResponseExceptionFilter : IAsyncExceptionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await context.HttpContext.Response.WriteAsJsonAsync(new ApiErrorResponse
        {
            FailureMessage = "Apologies, something went wrong. Please refresh and try again later."
        });
    }
}