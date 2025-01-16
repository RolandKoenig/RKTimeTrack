using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Testing;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

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