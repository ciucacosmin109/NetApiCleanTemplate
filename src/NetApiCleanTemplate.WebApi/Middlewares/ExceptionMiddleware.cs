using System.Net;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is DomainException domainException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ErrorDetails() {
                StatusCode = context.Response.StatusCode,
                Message = domainException.Message
            }.ToString());
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetails() {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        }.ToString());
    }
}

