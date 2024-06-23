using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Application.Util;
using RKTimeTrack.Service.Mappings;

namespace RKTimeTrack.Service.Api.Ui;

static class WeekApi
{
    [ProducesResponseType(typeof(TimeTrackingWeek), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetCurrentWeekAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetWeekUseCase useCase,
        CancellationToken cancellationToken)
    {
        // Map request
        var now = DateTime.UtcNow;
        var request = new GetWeekRequest(
            now.Year, 
            GermanCalendarWeekUtil.GetCalendarWeek(DateOnly.FromDateTime(now)));
        
        // Call application logic
        var result = await useCase.GetWeekAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
    
    [ProducesResponseType(typeof(TimeTrackingWeek), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetWeekAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetWeekUseCase useCase,
        [FromRoute] int year, 
        [FromRoute] int weekNumber,
        CancellationToken cancellationToken)
    {
        // Map request
        var request = new GetWeekRequest(year, weekNumber);
        
        // Call application logic
        var result = await useCase.GetWeekAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
}