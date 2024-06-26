﻿using ClickStitch.Middleware.Authentication;
using DotNetLibs.Api.Types;
using Inkwell.Client;
using Inkwell.Client.Types;
using System.Net;

namespace ClickStitch.Middleware.ExceptionHandling;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly IInkwellClient _inkwell;

    public ExceptionHandlingMiddleware(IInkwellClient inkwell)
    {
        _inkwell = inkwell;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await _inkwell.Log(new CreateLogRequest
            {
                LogLevel = InkwellLogLevel.Error,
                Message = exception.Message,
                StackTrace = exception.ToString(),
                JsonData = new
                {
                    User = RequestHelper.GetOptionalUser(context.Request)
                }
            });

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new ApiErrorResponse("Apologies, something went wrong. Please refresh and try again later."));
        }
    }
}