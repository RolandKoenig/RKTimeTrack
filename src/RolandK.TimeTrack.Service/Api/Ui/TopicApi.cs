using Microsoft.AspNetCore.Mvc;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Service.Api.Ui;

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