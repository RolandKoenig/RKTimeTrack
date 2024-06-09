using Microsoft.Extensions.DependencyInjection;

namespace RKTimeTrack.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRKTimeTrackApplication(this IServiceCollection services)
    {
        return services;
    }
}