using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Service.Mappings;

namespace RKTimeTrack.Service.Api.Ui;

static class TopicApi
{
    [ProducesResponseType(typeof(IReadOnlyCollection<TimeTrackingTopic>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetAllTopicsAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetAllTopics_UseCase useCase,
        CancellationToken cancellationToken)
    {
        var request = new GetAllTopics_Request();
        
        // Call application logic
        var result = await useCase.GetAllTopicsAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToResult(environment));
    }
}