namespace RolandK.TimeTrack.Service.Security;

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
            context.Response.Headers.AddSecurityHeaders();
            return Task.CompletedTask;
        });

        await _next(context);
    }
}