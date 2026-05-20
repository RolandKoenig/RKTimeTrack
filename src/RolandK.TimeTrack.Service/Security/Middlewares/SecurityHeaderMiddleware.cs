using RolandK.TimeTrack.Service.Security.Services;

namespace RolandK.TimeTrack.Service.Security.Middlewares;

public class SecurityHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            var nonceGenerator = context.RequestServices.GetRequiredService<ICspNonceGenerator>();

            var headers = context.Response.Headers;
            headers.TryAdd("X-Frame-Options", "DENY");
            headers.TryAdd("X-XSS-Protection", "0");
            headers.TryAdd("X-Content-Type-Options", "nosniff");
            headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
            headers.TryAdd("Cross-Origin-Embedder-Policy", "require-corp");
            headers.TryAdd("Cross-Origin-Resource-Policy", "same-site");
            headers.TryAdd("Permissions-Policy", "geolocation=(), camera=(), microphone=(), interest-cohort=()");
            headers.TryAdd("X-Robots-Tag", "noindex, nofollow");
        
            var nonce = nonceGenerator.GetNonceForCurrentScope();
            headers.TryAdd("Content-Security-Policy", 
                "default-src 'self';" +
                $"style-src 'self' 'nonce-{nonce}'");
            
            return Task.CompletedTask;
        });

        await _next(context);
    }
}