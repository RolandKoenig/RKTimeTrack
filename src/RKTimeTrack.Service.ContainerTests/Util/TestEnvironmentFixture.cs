using System.Diagnostics.CodeAnalysis;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

namespace RKTimeTrack.Service.ContainerTests.Util;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class TestEnvironmentFixture : IAsyncDisposable
{
    private IContainer[]? _containers;
    private IFutureDockerImage[]? _builtImages;
    
    private string? _appBaseUrl;
    
    public string AppBaseUrl => _appBaseUrl ?? string.Empty;
    
    public async Task EnsureContainersLoadedAsync()
    {
        if (_containers != null) { return; }
        
        var solutionDirectory = TestUtil.GetSolutionDirectory();
        var appImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(solutionDirectory)
            .WithDockerfile("src/RKTimeTrack.Service/Dockerfile")
            .WithBuildArgument("RESOURCE_REAPER_SESSION_ID", ResourceReaper.DefaultSessionId.ToString("D"))
            .Build();
        await appImage.CreateAsync();
        _builtImages = [appImage];

        var appContainer = new ContainerBuilder()
            .WithImage(appImage)
            .WithPortBinding(80, assignRandomHostPort: true)
            .WithEnvironment("Kestrel__Endpoints__Http__Url", "http://+:80")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "IntegrationTests")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Now listening on: http:\\/\\/\\[::]:80"))
            .Build();

        // Start all containers
        await appContainer.StartAsync();

        var appPort = appContainer.GetMappedPublicPort(80);
        _appBaseUrl = $"http://localhost:{appPort}";
        
        _containers = [appContainer];
    }

    public async Task WriteAppLogsAsync(Action<string> logLineWriter)
    {
        if ((_containers == null) ||
            (_containers.Length == 0))
        {
            return;
        }
        
        var logs = await _containers[0].GetLogsAsync();

        if (!string.IsNullOrEmpty(logs.Stdout))
        {
            logLineWriter("");
            logLineWriter("################### Stdout:");
            logLineWriter(logs.Stdout);
        }

        if (!string.IsNullOrEmpty(logs.Stderr))
        {
            logLineWriter("");
            logLineWriter("################### Stderr:");
            logLineWriter(logs.Stderr);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_containers != null)
        {
            foreach (var actContainer in _containers)
            {
                await actContainer.DisposeAsync();
            }
            _containers = null;
        }

        if (_builtImages != null)
        {
            foreach (var actImage in _builtImages)
            {
                await actImage.DeleteAsync();
                await actImage.DisposeAsync();
            }
            _builtImages = null;
        }
    }
}