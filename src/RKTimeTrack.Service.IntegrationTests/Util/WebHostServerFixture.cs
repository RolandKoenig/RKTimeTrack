using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using NSubstitute;
using NSubstitute.ClearExtensions;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Testing;
using Serilog;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests.Util;

// ReSharper disable once ClassNeverInstantiated.Global
public class WebHostServerFixture : IDisposable
{
    private static readonly DateTimeOffset _mockedStartTimestamp = new DateTimeOffset(
        2024, 12, 16, 11, 0, 0, TimeSpan.FromHours(1));
    private readonly Lazy<Uri> _rootUriInitializer;

    /// <summary>
    /// Dependency for startup
    /// </summary>
    public Func<string[], Action<WebApplicationBuilder>?, Action<LoggerConfiguration>?, IHost>? ProgramStartupMethod { get; set; }
    
    /// <summary>
    /// Dependency for logging into test results
    /// </summary>
    public ITestOutputHelper? TestOutputHelper { get; set; }

    public DateTimeOffset MockedStartTimestamp => _mockedStartTimestamp;
    
    public ITopicRepository TopicRepositoryMock { get; } = Substitute.For<ITopicRepository>();

    public ResettableFakeTimeProvider TimeProviderMock { get; } = new(_mockedStartTimestamp);
    
    public Uri RootUri => _rootUriInitializer.Value;
    
    private IHost? Host { get; set; }
    
    public WebHostServerFixture()
    {
        _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));
    }

    /// <summary>
    /// Open a Playwright session and navigate to root page.
    /// </summary>
    public async Task<PlaywrightSession> StartPlaywrightSessionOnRootPageAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = TestSettings.HEADLESS_MODE,
            SlowMo = TestSettings.SLOW_MODE_MILLISECONDS
        });
        
        var page = await browser.NewPageAsync();
        
        // Set browser clock to backend clock
        await page.Clock.SetFixedTimeAsync(this.MockedStartTimestamp.UtcDateTime);
        
        await page.GotoAsync(this.RootUri.AbsoluteUri);
        
        return new PlaywrightSession(playwright, browser, page);
    }

    public void Reset()
    {
        if (this.Host == null) { return; }
        
        var fileBasedTimeTrackingRepositoryTestInterface = 
            this.Host!.Services.GetRequiredService<IFileBasedTimeTrackingRepositoryTestInterface>();
        fileBasedTimeTrackingRepositoryTestInterface.ResetRepository();
        
        this.TopicRepositoryMock.ClearSubstitute();
        this.TimeProviderMock.Reset();
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
            webAppBuilder =>
            {
                var services = webAppBuilder.Services;
                
                services.AddFileBasedTimeTrackingRepositoryTestInterface();

                services.Replace(ServiceDescriptor.Singleton(TopicRepositoryMock));
                services.Replace(ServiceDescriptor.Singleton<TimeProvider>(TimeProviderMock));
            },
            loggerConfig =>
            {
                loggerConfig.WriteTo.Sink(new TestLoggerSink(this));
            });
    }
        
    public virtual void Dispose()
    {
        Host?.Dispose();
        Host?.StopAsync();
    }
}