using Microsoft.AspNetCore.Mvc;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Service.Api.Ui;

public class StateApi
{
    [ProducesResponseType(typeof(TimeTrackApplicationStatePublic), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetStateAsync(
        [FromServices] GetApplicationState_UseCase useCase,
        CancellationToken cancellationToken)
    {
        // Call application logic
        var result = await useCase.GetStateAsync(cancellationToken);

        // Map response
        return Results.Ok(result);
    }
}