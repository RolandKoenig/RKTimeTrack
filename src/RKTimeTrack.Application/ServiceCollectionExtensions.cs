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
            .AddScoped<GetAllTopicsUseCase>()
            .AddScoped<GetWeekUseCase>()
            .AddScoped<GetYearMetadataUseCase>()
            .AddScoped<SearchEntriesByTextUseCase>()
            .AddScoped<UpdateDayUseCase>();

        return services;
    }
}