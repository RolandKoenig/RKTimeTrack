namespace RolandK.TimeTrack.Service.Security;

public static class SecurityHeaders
{
    public static void ConfigureSecurityHeaders(this WebApplicationBuilder webHostBuilder)
    {
        webHostBuilder.WebHost.UseKestrel(x =>
        {
            x.AddServerHeader = false;
        });
    }

    public static void AddSecurityHeaders(this IHeaderDictionary headers)
    {
        headers.TryAdd("X-Frame-Options", "DENY");

        // Modern browsers ignore X-XSS-Protection or may behave unexpectedly with it.
        // Explicitly disabling it is recommended when using a proper CSP.
        headers.TryAdd("X-XSS-Protection", "0");
        
        headers.TryAdd(
            "Content-Security-Policy",
            string.Join("; ",
                "default-src 'self'",
                "base-uri 'self'",
                "object-src 'none'",
                "frame-ancestors 'none'",
                "form-action 'self'",
                "img-src 'self'",
                "font-src 'self'",
                "style-src 'self'",
                "script-src 'self'",
                "connect-src 'self'",
                "upgrade-insecure-requests"));
    }
}