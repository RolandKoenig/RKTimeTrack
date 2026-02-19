using Microsoft.Extensions.DependencyInjection;

namespace RolandK.TimeTrack.ExportAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTimeTrackingExportService(this IServiceCollection services)
    {
     
        return services;
    }
}