using Microsoft.Extensions.DependencyInjection;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRolandKTimeTrackApplication(this IServiceCollection services)
    {
        // UseCases
        services
            .AddScoped<GetAllTopics_UseCase>()
            .AddScoped<GetWeek_UseCases>()
            .AddScoped<GetYearMetadata_UseCase>()
            .AddScoped<SearchEntriesByText_UseCase>()
            .AddScoped<UpdateDay_UseCase>();

        return services;
    }
}