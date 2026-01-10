using Microsoft.Extensions.DependencyInjection;

namespace RolandK.RemoteFileStorage;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRemoteFileDataAccess(Action<FileDataStoreOptions> configure)
        {
            var options = new FileDataStoreOptions();
            configure(options);
            
            services.AddSingleton(options);
            services.AddScoped<IFileDataStore, S3FileDataStore>();
            
            return services;
        }
    }
}