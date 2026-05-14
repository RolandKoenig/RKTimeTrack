using Microsoft.AspNetCore.Mvc;
using RolandK.TimeTrack.Service.Mappings;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Service.Api.Ui;

static class DayApi
{
    [ProducesResponseType(typeof(TimeTrackingDay), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> UpdateDayAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] UpdateDay_UseCase useCase,
        [FromBody] UpdateDay_Request request,
        CancellationToken cancellationToken)
    {
        // Call application logic
        var result = await useCase.UpdateDayAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToBadRequestResult(environment));
    }
}