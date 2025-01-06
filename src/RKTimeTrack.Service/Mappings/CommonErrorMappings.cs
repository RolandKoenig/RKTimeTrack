using Microsoft.AspNetCore.Mvc;
using RKTimeTrack.Application;

namespace RKTimeTrack.Service.Mappings;

public static class CommonErrorMappings
{
    public static IResult ToBadRequestResult(
        this CommonErrors.ValidationError error,
        IWebHostEnvironment env)
    {
        if (env.IsProduction())
        {
            return Results.BadRequest();
        }
        
        return Results.BadRequest(new ProblemDetails()
        {
            Title = "Validation error",
            Detail = string.Join(
                ' ', 
                error.errors.Select(x => x.ToString()))
        });
    }
}