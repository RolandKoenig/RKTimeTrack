using RolandK.TimeTrack.Service.Security.Middlewares;
using RolandK.TimeTrack.Service.Security.Services;

namespace RolandK.TimeTrack.Service.Security;

public static class SecurityHeaders
{
    public static void ConfigureSecurityHeaders(this WebApplicationBuilder webHostBuilder)
    {
        webHostBuilder.Services.AddScoped<ICspNonceGenerator, DefaultCspNonceGenerator>();
        
        webHostBuilder.WebHost.UseKestrel(x =>
        {
            x.AddServerHeader = false;
        });
    }

    public static void UseSecurityHeadersAndMapIndexFile(this IApplicationBuilder applicationBuilder)
    {
        // Common security headers
        applicationBuilder.UseMiddleware<SecurityHeaderMiddleware>();
        
        // Map index file and inject nonce for Content-Security-Policy
        applicationBuilder.UseMiddleware<InjectCspNonceIntoIndexHtmlMiddleware>();
    }
}