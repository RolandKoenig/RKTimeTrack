using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileBasedTimeTrackingRepository(
        this IServiceCollection services,
        Action<FileBasedTimeTrackingRepositoryOptions> configure)
    {
        var options = new FileBasedTimeTrackingRepositoryOptions();
        configure(options);
        
        // Ensure single instance of FileBasedTimeTrackingRepository
        services
            .AddSingleton<FileBasedTimeTrackingRepository>()
            .AddSingleton<ITimeTrackingRepository, FileBasedTimeTrackingRepository>(x =>
                x.GetRequiredService<FileBasedTimeTrackingRepository>());
        
        return services
            .AddSingleton(options)
            .AddHostedService<TimeTrackingPersistenceService>();
    }
}