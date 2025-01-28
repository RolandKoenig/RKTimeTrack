using System.Diagnostics.CodeAnalysis;
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
    private static readonly DateTimeOffset _mockedStartTimestamp = new(
        2024, 12, 16, 11, 0, 0, TimeSpan.FromHours(1));
    private readonly Lazy<Uri> _rootUriInitializer;

    private IHost? _host;
    private IBrowser? _browser;
    private IPlaywright? _playwright;
    
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
    
    public WebHostServerFixture()
    {
        _rootUriInitializer = new Lazy<Uri>(() => new Uri(StartAndGetRootUri()));
    }

    /// <summary>
    /// Open a Playwright session and navigate to root page.
    /// </summary>
    public async Task<PlaywrightSession> StartPlaywrightSessionOnRootPageAsync()
    {
        _playwright ??= await Playwright.CreateAsync();
        _browser ??= await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = TestSettings.HEADLESS_MODE,
            SlowMo = TestSettings.SLOW_MODE_MILLISECONDS
        });
        
        Assertions.SetDefaultExpectTimeout(
            (float)TestSettings.DEFAULT_EXPECT_TIMEOUT.TotalMilliseconds);
        
        var page = await _browser.NewPageAsync();
        
        // Set browser clock to backend clock
        await page.Clock.SetFixedTimeAsync(this.MockedStartTimestamp.UtcDateTime);
        
        await page.GotoAsync(this.RootUri.AbsoluteUri);
        
        return new PlaywrightSession(page);
    }

    public void Reset()
    {
        if (_host == null) { return; }
        
        var fileBasedTimeTrackingRepositoryTestInterface = 
            _host!.Services.GetRequiredService<IFileBasedTimeTrackingRepositoryTestInterface>();
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
        _host = CreateWebHost();
        RunInBackgroundThread(_host.Start);
        return _host.Services
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

    [SuppressMessage("ReSharper", "AsyncVoidMethod")]
    public async void Dispose()
    {
        if (_host != null)
        {
            try
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            catch
            {
                // Nothing to do...
            }
        }

        if (_browser != null)
        {
            try
            {
                await _browser.DisposeAsync();
            }
            catch
            {
                // Nothing to do...
            }
        }

        if (_playwright != null)
        {
            try
            {
                _playwright.Dispose();
            }
            catch (Exception)
            {
                // Nothing to do
            }
        }
    }
}