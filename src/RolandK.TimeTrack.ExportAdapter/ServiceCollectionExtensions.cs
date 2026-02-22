using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RolandK.TimeTrack.Application.Ports;

namespace RolandK.TimeTrack.ExportAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTimeTrackingExportAdapter(
        this IServiceCollection services,
        IConfiguration parentConfigurationNode)
    {
        services.AddOptions<ExportAdapterOptions>()
            .Bind(parentConfigurationNode.GetSection(ExportAdapterOptions.SECTION_NAME))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddScoped<ITimeTrackingExporter, TimeTrackingExporter>();
        
        return services;
    }
}