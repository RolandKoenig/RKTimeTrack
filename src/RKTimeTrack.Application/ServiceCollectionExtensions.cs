using Microsoft.Extensions.DependencyInjection;
using RKTimeTrack.Application.UseCases;

namespace RKTimeTrack.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRKTimeTrackApplication(this IServiceCollection services) => services
        .AddScoped<GetAllTopicsUseCase>()
        .AddScoped<GetWeekUseCase>()
        .AddScoped<GetYearMetadataUseCase>()
        .AddScoped<UpdateDayUseCase>();
}