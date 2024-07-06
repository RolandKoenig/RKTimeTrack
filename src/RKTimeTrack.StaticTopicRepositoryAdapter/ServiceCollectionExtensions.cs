using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.StaticTopicRepositoryAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticTopicRepositoryAdapter(
        this IServiceCollection services,
        Action<StaticTopicRepositoryOptions> configure)
    {
        var options = new StaticTopicRepositoryOptions();
        configure(options);

        services.AddSingleton<StaticTopicRepositoryOptions>(options);
        services.AddSingleton<ITopicRepository, StaticTopicRepository>();

        return services;
    }
}