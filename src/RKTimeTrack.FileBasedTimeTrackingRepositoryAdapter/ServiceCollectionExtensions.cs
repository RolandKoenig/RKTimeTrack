using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Testing;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileBasedTimeTrackingRepository(
        this IServiceCollection services,
        Action<FileBasedTimeTrackingRepositoryOptions> configure)
    {
        var options = new FileBasedTimeTrackingRepositoryOptions();
        configure(options);
        
        services
            .AddSingleton<FileBasedTimeTrackingRepository>()
            .AddSingleton<ITimeTrackingRepository, FileBasedTimeTrackingRepository>(x =>
                x.GetRequiredService<FileBasedTimeTrackingRepository>());
        
        return services
            .AddSingleton(options)
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