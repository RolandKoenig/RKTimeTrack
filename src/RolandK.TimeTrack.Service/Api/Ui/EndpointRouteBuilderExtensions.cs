namespace RolandK.TimeTrack.Service.Api.Ui;

public static class EndpointRouteBuilderExtensions
{
    public static T MapWeekApi<T>(this T endpointBuilder)
        where T : IEndpointRouteBuilder
    {
        // Year Api
        endpointBuilder.MapGet("/api/ui/year/{year}/metadata", YearApi.GetYearMetadataAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(YearApi.GetYearMetadataAsync)));
        
        // Week Api
        endpointBuilder.MapGet("/api/ui/week", WeekApi.GetCurrentWeekAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(WeekApi.GetCurrentWeekAsync)));
        endpointBuilder.MapGet("/api/ui/week/{year}/{weekNumber}", WeekApi.GetWeekAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(WeekApi.GetWeekAsync)));
        
        // Day Api
        endpointBuilder.MapPost("/api/ui/day", DayApi.UpdateDayAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(DayApi.UpdateDayAsync)));
        
        // Entry Api
        endpointBuilder.MapPost("/api/ui/entries", EntryApi.SearchEntriesAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(EntryApi.SearchEntriesAsync)));

        // State Api
        endpointBuilder.MapGet("/api/ui/state", StateApi.GetStateAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(StateApi.GetStateAsync)));
        
        // Topic Api
        endpointBuilder.MapGet("/api/ui/topics", TopicApi.GetAllTopicsAsync)
            .AddNoStoreCacheHeader()
            .WithName(RemoveAsyncFromMethodName(nameof(TopicApi.GetAllTopicsAsync)));
        
        return endpointBuilder;
    }

    private static string RemoveAsyncFromMethodName(string methodName)
    {
        if (!methodName.EndsWith("Async", StringComparison.OrdinalIgnoreCase)) { return methodName; }
        return methodName.Substring(0, methodName.Length - 5);
    }
}