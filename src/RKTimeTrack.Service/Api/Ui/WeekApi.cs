using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Service.Mappings;

namespace RKTimeTrack.Service.Api.Ui;

static class WeekApi
{
    [ProducesResponseType(typeof(TimeTrackingWeek), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetCurrentWeekAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] TimeProvider timeProvider,
        [FromServices] GetWeek_UseCases useCases,
        CancellationToken cancellationToken)
    {
        // Call application logic
        var result = await useCases.GetCurrentWeekAsync(cancellationToken);

        // Map response
        return Results.Ok(result);
    }
    
    [ProducesResponseType(typeof(TimeTrackingWeek), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetWeekAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetWeek_UseCases useCases,
        [FromRoute] int year, 
        [FromRoute] int weekNumber,
        CancellationToken cancellationToken)
    {
        // Map request
        var request = new GetWeek_Request(year, weekNumber);
        
        // Call application logic
        var result = await useCases.GetWeekAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToBadRequestResult(environment));
    }
}