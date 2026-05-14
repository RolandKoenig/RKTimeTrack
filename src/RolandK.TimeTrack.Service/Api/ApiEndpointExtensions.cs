namespace RolandK.TimeTrack.Service.Api;

public static class ApiEndpointExtensions
{
    public static RouteHandlerBuilder AddNoStoreCacheHeader(this RouteHandlerBuilder routeHandlerBuilder)
        => routeHandlerBuilder.AddEndpointFilter(async (context, next) =>
        {
            var isGetOrHeadRequest = HttpMethods.IsGet(context.HttpContext.Request.Method) || 
                                     HttpMethods.IsHead(context.HttpContext.Request.Method);
            if (!isGetOrHeadRequest)
            {
                return await next(context);
            }

            var result = await next(context);
            if (context.HttpContext.Response.StatusCode == StatusCodes.Status200OK)
            {
                context.HttpContext.Response.Headers.CacheControl = "no-store";
            }

            return result;
        });
}