using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Service.Mappings;

namespace RKTimeTrack.Service.Api.Ui;

static class YearApi
{
    [ProducesResponseType(typeof(TimeTrackingYearMetadata), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    internal static async Task<IResult> GetYearMetadataAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromServices] GetYearMetadata_UseCase useCase,
        [FromRoute] int year = 0, 
        CancellationToken cancellationToken = default)
    {
        // Map request
        var request = new GetYearMetadata_Request(year);
        
        // Call application logic
        var result = await useCase.GetYearMetadataAsync(request, cancellationToken);

        // Map response
        return result.Match(
            Results.Ok,
            validationError => validationError.ToBadRequestResult(environment));
    }
}