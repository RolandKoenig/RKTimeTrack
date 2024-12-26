using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.Services;
using RKTimeTrack.Application.UseCases;

namespace RKTimeTrack.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRKTimeTrackApplication(this IServiceCollection services)
    {
        // Services
        services
            .AddSingleton<IClock, DefaultClock>();

        // UseCases
        services
            .AddScoped<GetAllTopics_UseCase>()
            .AddScoped<GetWeek_UseCase>()
            .AddScoped<GetYearMetadata_UseCase>()
            .AddScoped<SearchEntriesByText_UseCase>()
            .AddScoped<UpdateDay_UseCase>();

        return services;
    }
}