using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests.Util;

// ReSharper disable once ClassNeverInstantiated.Global
public class WebHostServerFixture : IDisposable
{
    private readonly Lazy<Uri> _rootUriInitializer;

    /// <summary>
    /// Dependency for startup
    /// </summary>
    public Func<string[], Action<WebApplicationBuilder>?, IHost>? ProgramStartupMethod { get; set; }
    
    /// <summary>
    /// Dependency for logging into test results
    /// </summary>
    public ITestOutputHelper? TestOutputHelper { get; set; }
    
    public Uri RootUri => _rootUriInitializer.Value;
    
    private IHost? Host { get; set; }
    
    public WebHostServerFixture()
    {
        _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));
    }

    private static void RunInBackgroundThread(Action action)
    {
        using var isDone = new ManualResetEvent(false);

        ExceptionDispatchInfo? edi = null;
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

    private string StartAndGetRootUri()
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
                // Configure logging 
                // (we want to see logging-output in the test results)
                builder.Logging.AddProvider(new TestLoggerProvider(this));
            });
    }
        
    public virtual void Dispose()
    {
        Host?.Dispose();
        Host?.StopAsync();
    }
}