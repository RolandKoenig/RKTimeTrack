using Microsoft.OpenApi.Models;
using RolandK.TimeTrack.Application.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RolandK.TimeTrack.Service.Mappings;

public static class SwaggerGenConfiguration
{
    public static void Configure(SwaggerGenOptions options)
    {
        options.MapType<TimeTrackingHours>(() => new OpenApiSchema(){ Type = "number" });
        options.MapType<TimeTrackingBudget>(() => new OpenApiSchema(){ Type = "number" });
        options.MapType<DateOnly>(() => new OpenApiSchema()
        {
            Type = "string",
            Description = "Date in format 'yyyy-mm-dd'"
        });
    }
}