using Microsoft.OpenApi;
using RolandK.TimeTrack.Application.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RolandK.TimeTrack.Service.Mappings;

public static class SwaggerGenConfiguration
{
    public static void Configure(SwaggerGenOptions options)
    {
        options.MapType<TimeTrackingHours>(() => new OpenApiSchema(){ Type = JsonSchemaType.Number });
        options.MapType<TimeTrackingBudget>(() => new OpenApiSchema(){ Type = JsonSchemaType.Number });
        options.MapType<DateOnly>(() => new OpenApiSchema()
        {
            Type = JsonSchemaType.String,
            Description = "Date in format 'yyyy-mm-dd'"
        });
    }
}