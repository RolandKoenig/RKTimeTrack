using Microsoft.AspNetCore.Mvc;
using RolandK.TimeTrack.Service.Mappings;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Service.Api.Ui;

static class EntryApi
{
    [ProducesResponseType(typeof(IReadOnlyList<TimeTrackingEntry>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> SearchEntriesAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] SearchEntriesByText_UseCase useCase,
        [FromBody] SearchEntriesByText_Request request,
        CancellationToken cancellationToken)
    {
        // Call application logic
        var result = 
            await useCase.SearchEntriesByTextAsync(request, cancellationToken);
        
        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToBadRequestResult(environment));
    }
}