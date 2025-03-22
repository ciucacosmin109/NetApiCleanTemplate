using Swashbuckle.AspNetCore.SwaggerUI;

namespace NetApiCleanTemplate.WebApi.Middlewares;

public static class HstsMiddleware
{
    public static void UseHstsMiddleware(this IApplicationBuilder app, IConfiguration config)
    {
        var enable = config.GetValue<bool>("Security:UseHsts");
        if (!enable)
        {
            return;
        }

        app.UseHsts();
    }
}
