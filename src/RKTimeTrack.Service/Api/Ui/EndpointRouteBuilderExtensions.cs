namespace RKTimeTrack.Service.Api.Ui;

public static class EndpointRouteBuilderExtensions
{
    public static T MapWeekApi<T>(this T endpointBuilder)
        where T : IEndpointRouteBuilder
    {
        endpointBuilder.MapGet("/api/ui/week", WeekApi.GetWeek)
            .WithName("GetWeek")
            .WithOpenApi();
        
        return endpointBuilder;
    }
}