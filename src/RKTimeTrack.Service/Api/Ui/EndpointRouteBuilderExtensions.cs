namespace RKTimeTrack.Service.Api.Ui;

public static class EndpointRouteBuilderExtensions
{
    public static T MapWeekApi<T>(this T endpointBuilder)
        where T : IEndpointRouteBuilder
    {
        // Year Api
        endpointBuilder.MapGet("/api/ui/year/{year}/metadata", YearApi.GetYearMetadataAsync)
            .WithName(RemoveAsyncFromMethodName(nameof(YearApi.GetYearMetadataAsync)))
            .WithOpenApi();
        
        // Week Api
        endpointBuilder.MapGet("/api/ui/week", WeekApi.GetCurrentWeekAsync)
            .WithName(RemoveAsyncFromMethodName(nameof(WeekApi.GetCurrentWeekAsync)))
            .WithOpenApi();
        endpointBuilder.MapGet("/api/ui/week/{year}/{weekNumber}", WeekApi.GetWeekAsync)
            .WithName(RemoveAsyncFromMethodName(nameof(WeekApi.GetWeekAsync)))
            .WithOpenApi();
        
        // Day Api
        endpointBuilder.MapPost("/api/ui/day", DayApi.UpdateDayAsync)
            .WithName(RemoveAsyncFromMethodName(nameof(DayApi.UpdateDayAsync)))
            .WithOpenApi();
        
        // Topic Api
        endpointBuilder.MapGet("/api/ui/topics", TopicApi.GetAllTopicsAsync)
            .WithName(RemoveAsyncFromMethodName(nameof(TopicApi.GetAllTopicsAsync)))
            .WithOpenApi();
        
        return endpointBuilder;
    }

    private static string RemoveAsyncFromMethodName(string methodName)
    {
        if (!methodName.EndsWith("Async", StringComparison.OrdinalIgnoreCase)) { return methodName; }
        return methodName.Substring(0, methodName.Length - 5);
    }
}