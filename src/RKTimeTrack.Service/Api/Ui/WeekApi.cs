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
        [FromServices] TimeProvider timeProvider,
        [FromServices] GetWeek_UseCase useCase,
        CancellationToken cancellationToken)
    {
        // Map request
        var now = timeProvider.GetUtcNow();
        var weekOfYear = GermanCalendarWeekUtil.GetCalendarWeek(
            DateOnly.FromDateTime(now.DateTime),
            out var nextYear);
        var year = nextYear ? now.Year + 1 : now.Year;
        
        var request = new GetWeek_Request(
            year, 
            weekOfYear);
        
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
        [FromServices] GetWeek_UseCase useCase,
        [FromRoute] int year, 
        [FromRoute] int weekNumber,
        CancellationToken cancellationToken)
    {
        // Map request
        var request = new GetWeek_Request(year, weekNumber);
        
        // Call application logic
        var result = await useCase.GetWeekAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
}