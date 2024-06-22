namespace RKTimeTrack.Service.Api.Ui;

public static class EndpointRouteBuilderExtensions
{
    public static T MapWeekApi<T>(this T endpointBuilder)
        where T : IEndpointRouteBuilder
    {
        // Week Api
        endpointBuilder.MapGet("/api/ui/week", WeekApi.GetCurrentWeek)
            .WithName(nameof(WeekApi.GetCurrentWeek))
            .WithOpenApi();
        endpointBuilder.MapGet("/api/ui/week/{year}/{weekNumber}", WeekApi.GetWeek)
            .WithName(nameof(WeekApi.GetWeek))
            .WithOpenApi();
        
        // Year Api
        endpointBuilder.MapGet("/api/ui/year/{year}/metadata", YearApi.GetYearMetadata)
            .WithName(nameof(YearApi.GetYearMetadata))
            .WithOpenApi();
        
        return endpointBuilder;
    }
}