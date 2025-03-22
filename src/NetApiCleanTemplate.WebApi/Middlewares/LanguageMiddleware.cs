using System.Globalization;
using NetApiCleanTemplate.Infrastructure.Session;
using NetApiCleanTemplate.SharedKernel.Interfaces.Session;

namespace NetApiCleanTemplate.WebApi.Middlewares;

public static class LanguageMiddleware
{
    public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder app)
    {
        return app.Use((context, next) =>
        {
            var languageCode = context.Request.Headers[LanguageService.LanguageCodeHeader].ToString();

            var langServ = context.RequestServices.GetRequiredService<ILanguageService>();
            langServ.SetCode(languageCode);

            CultureInfo.CurrentCulture = new CultureInfo(languageCode);
            CultureInfo.CurrentUICulture = new CultureInfo(languageCode);

            return next.Invoke();
        });
    }
}