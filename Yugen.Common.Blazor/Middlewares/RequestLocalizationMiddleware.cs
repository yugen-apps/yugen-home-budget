using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace Yugen.Common.Blazor.Middlewares;

public class RequestLocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userLanguages = context.Request.Headers["Accept-Language"].ToString();
        if (!string.IsNullOrWhiteSpace(userLanguages))
        {
            var preferredLanguage = userLanguages.Split(',')[0];
            var culture = new CultureInfo(preferredLanguage);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        await _next(context);
    }
}