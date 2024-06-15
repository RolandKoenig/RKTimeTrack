using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.StaticTopicRepositoryAdapter;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticTopicRepositoryAdapter(this IServiceCollection services)
        => services.AddSingleton<ITopicRepository, StaticTopicRepository>();
}