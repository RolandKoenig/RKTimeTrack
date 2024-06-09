using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.StaticProjectRepositoryAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticProjectRepositoryAdapter(this IServiceCollection services)
        => services.AddSingleton<IProjectRepository, StaticProjectRepository>();
}