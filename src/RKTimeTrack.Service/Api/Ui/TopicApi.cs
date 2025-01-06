using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;

namespace RKTimeTrack.Service.Api.Ui;

static class TopicApi
{
    [ProducesResponseType(typeof(IReadOnlyCollection<TimeTrackingTopic>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetAllTopicsAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetAllTopics_UseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.GetAllTopicsAsync(cancellationToken);
        
        return Results.Ok(result);
    }
}