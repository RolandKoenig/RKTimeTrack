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

    public static void AddSecurityHeaders(this IHeaderDictionary headers, HttpRequest request)
    {
        // Common security headers
        // see https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Headers_Cheat_Sheet.html
        
        headers.TryAdd("X-Frame-Options", "DENY");
        headers.TryAdd("X-XSS-Protection", "0");
        headers.TryAdd("X-Content-Type-Options", "nosniff");
        headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
        headers.TryAdd("Cross-Origin-Embedder-Policy", "require-corp");
        headers.TryAdd("Cross-Origin-Resource-Policy", "same-site");
        headers.TryAdd("Permissions-Policy", "geolocation=(), camera=(), microphone=(), interest-cohort=()");
        headers.TryAdd("X-Robots-Tag", "noindex, nofollow");
        
        headers.TryAdd("Content-Security-Policy", "default-src 'self'");
    }
}