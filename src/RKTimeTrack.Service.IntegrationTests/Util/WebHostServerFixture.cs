using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests.Util;

public class WebHostServerFixture : IDisposable
{
    private readonly Lazy<Uri> _rootUriInitializer;

    public Uri RootUri => _rootUriInitializer.Value;
    
    public Func<string[], Action<WebApplicationBuilder>?, IHost>? ProgramStartupMethod { get; set; } = null;

    public ITestOutputHelper? TestOutputHelper { get; set; } = null;
    
    private IHost? Host { get; set; }
    
    public WebHostServerFixture()
    {
        _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));
    }

    private static void RunInBackgroundThread(Action action)
    {
        using var isDone = new ManualResetEvent(false);

        ExceptionDispatchInfo edi = null;
        new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            // ReSharper disable once AccessToDisposedClosure
            isDone.Set();
            
        }).Start();

        if (!isDone.WaitOne(TimeSpan.FromSeconds(10)))
        {
            throw new TimeoutException("Timed out waiting for: " + action);
        }

        if (edi != null)
        {
            throw edi.SourceException;
        }
    }

    protected string StartAndGetRootUri()
    {
        // As the port is generated automatically, we can use IServerAddressesFeature to get the actual server URL
        Host = CreateWebHost();
        RunInBackgroundThread(Host.Start);
        return Host.Services
            .GetRequiredService<IServer>()
            .Features
            .Get<IServerAddressesFeature>()!
            .Addresses.Single();
    }

    private IHost CreateWebHost()
    {
        if (this.ProgramStartupMethod == null)
        {
            throw new InvalidOperationException($"{nameof(ProgramStartupMethod)} not set!");
        }
        
        return this.ProgramStartupMethod(
            ["--environment", "IntegrationTests"], 
            builder =>
            {
                // Make UseStaticWebAssets work
                var applicationPath = typeof(Program).Assembly.Location;
                var name = Path.ChangeExtension(applicationPath, ".staticwebassets.runtime.json");
                var inMemoryConfiguration = new Dictionary<string, string>
                {
                    [WebHostDefaults.StaticWebAssetsKey] = name,
                };
                builder.Configuration.AddInMemoryCollection(inMemoryConfiguration);

                if (this.TestOutputHelper != null)
                {
                    builder.Logging.AddProvider(new TestLoggerProvider(this.TestOutputHelper));
                }

                builder.Logging.AddConsole();

                // builder.WebHost
                //     .UseKestrel()
                //     // .UseSolutionRelativeContentRoot(typeof(TStartup).Assembly.GetName().Name!)
                //     .UseStaticWebAssets();
                // // .UseStartup<TStartup>()
                // // .UseUrls($"http://127.0.0.1:0"); // :0 allows to choose a port automatically
            });

        // return new HostBuilder()
        //     .ConfigureHostConfiguration(config =>
        //     {
        //         // Make UseStaticWebAssets work
        //         var applicationPath = typeof(TStartup).Assembly.Location;
        //         var name = Path.ChangeExtension(applicationPath, ".staticwebassets.runtime.json");
        //         var inMemoryConfiguration = new Dictionary<string, string>
        //         {
        //             [WebHostDefaults.StaticWebAssetsKey] = name,
        //         };
        //         config.AddInMemoryCollection(inMemoryConfiguration);
        //     })
        //     .ConfigureWebHost(webHostBuilder => webHostBuilder
        //         .UseKestrel()
        //         .UseSolutionRelativeContentRoot(typeof(TStartup).Assembly.GetName().Name)
        //         .UseStaticWebAssets()
        //         .UseStartup<TStartup>()
        //         .UseUrls($"http://127.0.0.1:0")) // :0 allows to choose a port automatically
        //     .Build();
    }
        
    public virtual void Dispose()
    {
        Host?.Dispose();
        Host?.StopAsync();
    }
}

// ASP.NET Core with a Startup class (MVC / Pages / Blazor Server)
// public class WebHostServerFixture<TStartup> : WebHostServerFixture
//     where TStartup : class
// {
//     protected override IHost CreateWebHost()
//     {
//         return RKTimeTrack.Service.Program.CreateApplication(
//             ["--environment", "IntegrationTests"], 
//             builder =>
//             {
//                 // Make UseStaticWebAssets work
//                 var applicationPath = typeof(TStartup).Assembly.Location;
//                 var name = Path.ChangeExtension(applicationPath, ".staticwebassets.runtime.json");
//                 var inMemoryConfiguration = new Dictionary<string, string>
//                 {
//                     [WebHostDefaults.StaticWebAssetsKey] = name,
//                 };
//                 builder.Configuration.AddInMemoryCollection(inMemoryConfiguration);
// 
//                 builder.Logging.AddConsole();
// 
//                 // builder.WebHost
//                 //     .UseKestrel()
//                 //     // .UseSolutionRelativeContentRoot(typeof(TStartup).Assembly.GetName().Name!)
//                 //     .UseStaticWebAssets();
//                 // // .UseStartup<TStartup>()
//                 // // .UseUrls($"http://127.0.0.1:0"); // :0 allows to choose a port automatically
//             });
// 
//         // return new HostBuilder()
//         //     .ConfigureHostConfiguration(config =>
//         //     {
//         //         // Make UseStaticWebAssets work
//         //         var applicationPath = typeof(TStartup).Assembly.Location;
//         //         var name = Path.ChangeExtension(applicationPath, ".staticwebassets.runtime.json");
//         //         var inMemoryConfiguration = new Dictionary<string, string>
//         //         {
//         //             [WebHostDefaults.StaticWebAssetsKey] = name,
//         //         };
//         //         config.AddInMemoryCollection(inMemoryConfiguration);
//         //     })
//         //     .ConfigureWebHost(webHostBuilder => webHostBuilder
//         //         .UseKestrel()
//         //         .UseSolutionRelativeContentRoot(typeof(TStartup).Assembly.GetName().Name)
//         //         .UseStaticWebAssets()
//         //         .UseStartup<TStartup>()
//         //         .UseUrls($"http://127.0.0.1:0")) // :0 allows to choose a port automatically
//         //     .Build();
//     }
// }