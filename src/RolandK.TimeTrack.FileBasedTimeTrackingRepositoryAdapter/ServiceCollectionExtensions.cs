using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RolandK.TimeTrack.Application.Ports;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Testing;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileBasedTimeTrackingRepository(
        this IServiceCollection services,
        IConfiguration parentConfigurationNode)
    {
        services.AddOptions<FileBasedTimeTrackingRepositoryOptions>()
            .Bind(parentConfigurationNode.GetSection(FileBasedTimeTrackingRepositoryOptions.SECTION_NAME))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddSingleton<FileBasedTimeTrackingRepository>()
            .AddSingleton<ITimeTrackingRepository, FileBasedTimeTrackingRepository>(x =>
                x.GetRequiredService<FileBasedTimeTrackingRepository>());
        
        return services
            .AddSingleton<TimeTrackingStore>()
            .AddHostedService<TimeTrackingPersistenceService>();
    }
    
    public static IServiceCollection AddFileBasedTimeTrackingRepositoryTestInterface(
        this IServiceCollection services)
    {
        return services
            .AddSingleton<IFileBasedTimeTrackingRepositoryTestInterface, FileBasedTimeTrackingRepositoryTestInterface>();
    }
}