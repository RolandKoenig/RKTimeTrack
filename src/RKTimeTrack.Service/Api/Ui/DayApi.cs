using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Service.Mappings;

namespace RKTimeTrack.Service.Api.Ui;

static class DayApi
{
    [ProducesResponseType(typeof(TimeTrackingDay), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> UpdateDayAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] UpdateDayUseCase useCase,
        [FromBody] UpdateDayRequest request,
        CancellationToken cancellationToken = default)
    {
        // Call application logic
        var result = await useCase.UpdateDayAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
}