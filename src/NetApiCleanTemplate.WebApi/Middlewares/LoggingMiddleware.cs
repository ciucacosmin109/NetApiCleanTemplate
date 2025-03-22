using System.Net;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace NetApiCleanTemplate.WebApi.Middlewares;

public static class LoggingMiddleware
{
    public static void UseRequestLoggingMiddleware(this IApplicationBuilder application)
    {
        application
            .Use(async (httpContext, next) => {
                var claims = httpContext.User.Claims.ToList();
                var username = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.UserName)?.Value ?? "";
                var tenantId = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.SelectedTenant)?.Value ?? "";

                using (LogContext.PushProperty("UserName", username))
                using (LogContext.PushProperty("Tenant", tenantId))
                {
                    await next.Invoke(httpContext);
                }
            })
            .UseSerilogRequestLogging(options => {
                options.GetLevel = (context, elapsed, ex) => {
                    var isBadReq = context.Response.StatusCode >= (int)HttpStatusCode.BadRequest && context.Response.StatusCode < (int)HttpStatusCode.InternalServerError;
                    var isIse = context.Response.StatusCode >= (int)HttpStatusCode.InternalServerError;

                    // Bad request / Domain exception => Debug
                    if (isBadReq || (ex is not null && ex is DomainException))
                    {
                        return LogEventLevel.Debug;
                    }

                    // Internal Server Error / Any exception => Error
                    if (isIse || ex is not null)
                    {
                        return LogEventLevel.Error;
                    }

                    // Default => Debug
                    return LogEventLevel.Debug;
                };
            });
    }
}
