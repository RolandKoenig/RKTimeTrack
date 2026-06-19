using Microsoft.OpenApi;
using RolandK.TimeTrack.Application;
using RolandK.TimeTrack.ExportAdapter;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter;
using RolandK.TimeTrack.Service.Api.Tests;
using RolandK.TimeTrack.Service.Api.Ui;
using RolandK.TimeTrack.Service.BackgroundServices;
using RolandK.TimeTrack.StaticTopicRepositoryAdapter;
using RolandK.TimeTrack.Service.Mappings;
using RolandK.TimeTrack.Service.Security;
using Serilog;

namespace RolandK.TimeTrack.Service;

public static class Program
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
        Action<WebApplicationBuilder>? customizeWebApplicationBuilder = null,
        Action<LoggerConfiguration>? customizeLogger = null)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Load secrets directory (if configured)
        var secretsDirectory = builder.Configuration.GetSection("Startup").GetValue<string>("SecretsDirectory");
        if (Directory.Exists(secretsDirectory))
        {
            builder.Configuration
                .AddKeyPerFile(secretsDirectory);
        }
        
        // #########################################
        // Add services to the container.
        builder.Services.AddSerilog(
            config =>
            {
                config.WriteTo.Console();
                customizeLogger?.Invoke(config);
            });
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddHealthChecks();

        // Add application services
        builder.Services.AddRolandKTimeTrackApplication();
        
        // Add adapters
        builder.Services.AddFileBasedTimeTrackingRepository(builder.Configuration);
        builder.Services.AddStaticTopicRepositoryAdapter(
            options => builder.Configuration.Bind("StaticTopicRepository", options));
        builder.Services.AddTimeTrackingExportAdapter(builder.Configuration);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerGenConfiguration.Configure);

        // Add Background services
        builder.Services.AddHostedService<ExportTriggerBackgroundService>();
        
        // Security configuration
        builder.ConfigureSecurityHeaders();
        
        // Allow customization from IntegrationTests project
        customizeWebApplicationBuilder?.Invoke(builder);
        
        // #########################################
        // Configure the HTTP request pipeline.
        
        var app = builder.Build();
        app.UseSerilogRequestLogging();

        app.UseSecurityHeadersAndMapIndexFile();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0);
            app.UseSwaggerUI();
        }
        
        app.UseAuthorization();
        app.MapHealthChecks("/health");
        app.UseStaticFiles();
        
        // Our apis
        app.MapWeekApi();
        
        // Test-Endpoint
        // Used by RolandK.TimeTrack.Service.ContainerTests.StartupTests.Start_application_container_and_check_test_endpoint
        // This one is used to check, whether secrets are correctly loaded from the configured secrets directory
        if (builder.Configuration.GetSection("Startup").GetValue<bool>("SecretTestEndpointAvailable"))
        {
            app.MapTestEndpoint();
        }
        
        return app;
    }
}