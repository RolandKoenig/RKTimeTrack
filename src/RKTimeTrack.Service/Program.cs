using RKTimeTrack.Application;
using RKTimeTrack.Service.Api.Ui;
using RKTimeTrack.Service.Mappings;

namespace RKTimeTrack.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Add application services
        builder.Services.AddRKTimeTrackApplication();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerGenConfiguration.Configure);
        
        builder.Services.AddHttpLogging(o => { });

        var app = builder.Build();

        app.UseHttpLogging();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthorization();

        // Our apis
        app.MapWeekApi();
        
        // Run application        
        app.MapFallbackToFile("index.html");
        app.Run();
    }
}