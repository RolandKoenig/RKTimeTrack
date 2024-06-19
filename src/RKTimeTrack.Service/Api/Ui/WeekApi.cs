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
    internal static async Task<IResult> GetWeek(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetWeekUseCase useCase,
        [FromQuery] int year = 0, 
        [FromQuery] int weekNumber = 0,
        CancellationToken cancellationToken = default)
    {
        // Map request
        GetWeekRequest request;
        if ((year != 0) ||
            (weekNumber != 0))
        {
            request = new GetWeekRequest(year, weekNumber);
        }
        else
        {
            var now = DateTime.UtcNow;
            request = new GetWeekRequest(
                now.Year, 
                GermanCalendarWeekUtil.GetCalendarWeek(DateOnly.FromDateTime(now)));
        }
        
        // Call application logic
        var result = await useCase.GetWeekAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
}