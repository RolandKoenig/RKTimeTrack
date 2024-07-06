using RKTimeTrack.Application;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;
using RKTimeTrack.Service.Api.Ui;
using RKTimeTrack.Service.Mappings;
using RKTimeTrack.StaticTopicRepositoryAdapter;

namespace RKTimeTrack.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateApplication(args);
        host.Run();
    }

    /// <summary>
    /// Hook for IntegrationTests project
    /// </summary>
    internal static IHost CreateApplication(
        string[] args,
        Action<WebApplicationBuilder>? customizeWebApplicationBuilder = null)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // #########################################
        // Add services to the container.
        
        builder.Services.AddAuthorization();

        // Add application services
        builder.Services.AddRKTimeTrackApplication();
        
        // Add adapters
        builder.Services.AddFileBasedTimeTrackingRepository(
            options => builder.Configuration.Bind("FileBasedTimeTrackingRepository", options));
        builder.Services.AddStaticTopicRepositoryAdapter();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerGenConfiguration.Configure);
        
        builder.Services.AddHttpLogging(o => { });

        // Allow customization from IntegrationTests project
        customizeWebApplicationBuilder?.Invoke(builder);

        // #########################################
        // Configure the HTTP request pipeline.
        
        var app = builder.Build();
        app.UseHttpLogging();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthorization();
        app.UseStaticFiles();

        // Our apis
        app.MapWeekApi();
        
        // Run application        
        app.MapFallbackToFile("index.html");

        return app;
    }
}