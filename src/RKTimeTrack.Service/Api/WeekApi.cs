using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;

namespace RKTimeTrack.Service.Api;

public static class WeekApi
{
    public static T MapWeekApi<T>(this T endpointBuilder)
        where T : IEndpointRouteBuilder
    {
        endpointBuilder.MapGet("/api/week", GetWeek)
            .WithName("GetWeek")
            .WithOpenApi();
        
        return endpointBuilder;
    }
    
    [ProducesResponseType(typeof(TimeTrackingWeek), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> GetWeek(
        [FromServices] GetWeekUseCase useCase,
        [FromQuery, Required] int year,
        [FromQuery, Required] int weekNumber,
        CancellationToken cancellationToken)
    {
        var request = new GetWeekRequest(year, weekNumber);
        var result = await useCase.GetWeekAsync(request, cancellationToken);
        
        return result.Match(
            Results.Ok,
            validationError => Results.BadRequest());
    }
    
}