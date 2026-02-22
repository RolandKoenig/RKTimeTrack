using Microsoft.Extensions.DependencyInjection;
using RolandK.TimeTrack.Application.State;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRolandKTimeTrackApplication(this IServiceCollection services)
    {
        // UseCases
        services
            .AddScoped<ExportTimeTrackingData_UseCase>()
            .AddScoped<GetAllTopics_UseCase>()
            .AddScoped<GetWeek_UseCases>()
            .AddScoped<GetYearMetadata_UseCase>()
            .AddScoped<SearchEntriesByText_UseCase>()
            .AddScoped<UpdateDay_UseCase>();
        
        // State
        services
            .AddSingleton(new TimeTrackApplicationState());

        return services;
    }
}