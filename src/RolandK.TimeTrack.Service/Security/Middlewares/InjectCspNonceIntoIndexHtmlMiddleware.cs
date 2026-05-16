using Microsoft.Net.Http.Headers;
using RolandK.TimeTrack.Service.Security.Services;

namespace RolandK.TimeTrack.Service.Security.Middlewares;

public class InjectCspNonceIntoIndexHtmlMiddleware
{
    private const string CSP_NONCE_PLACEHOLDER = "**[CSP-NONCE]**";
    private const string INDEX_HTML_FILE = "index.html";

    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private string? _originalIndexContent;

    public InjectCspNonceIntoIndexHtmlMiddleware(
        RequestDelegate next,
        IWebHostEnvironment webHostEnvironment)
    {
        _next = next;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!HttpMethods.IsGet(context.Request.Method) ||
            !IsIndexHtmlRequest(context.Request))
        {
            await _next(context);
            return;
        }

        var fileInfo = _webHostEnvironment.WebRootFileProvider.GetFileInfo(INDEX_HTML_FILE);
        if (!fileInfo.Exists)
        {
            await _next(context);
            return;
        }

        var nonceGenerator = context.RequestServices.GetRequiredService<ICspNonceGenerator>();
        var nonce = nonceGenerator.GetNonceForCurrentScope();

        if (string.IsNullOrEmpty(_originalIndexContent))
        {
            // Read the file only once
            await using var fileStream = fileInfo.CreateReadStream();
            using var reader = new StreamReader(fileStream);
            
            _originalIndexContent = await reader.ReadToEndAsync();
        }
        var updatedIndexFileContent = _originalIndexContent.Replace(CSP_NONCE_PLACEHOLDER, nonce, StringComparison.Ordinal);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/html; charset=utf-8";
        context.Response.Headers[HeaderNames.CacheControl] = "no-cache";

        await context.Response.WriteAsync(updatedIndexFileContent);
    }

    private static bool IsIndexHtmlRequest(HttpRequest request)
    {
        if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
        {
            return false;
        }

        var path = request.Path;
        if (!path.HasValue)
        {
            return true;
        }

        if (path.Equals("/", StringComparison.OrdinalIgnoreCase) ||
            path.Equals("/index.html", StringComparison.OrdinalIgnoreCase) ||
            path.Equals("/index.htm", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return !Path.HasExtension(path.Value);
    }
}