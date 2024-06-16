using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
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
        [AsParameters] GetWeekRequest request,
        CancellationToken cancellationToken)
    {
        var result = await useCase.GetWeekAsync(request, cancellationToken);

        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
}