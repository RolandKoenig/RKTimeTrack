using System.Net.Http.Json;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class StateApiTests
{
    private readonly WebHostServerFixture _server;

    public StateApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }

    [Fact]
    public async Task GetCurrentState_no_export()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        var fakeStartupTimestamp = DateTime.UtcNow;
        _server.ApplicationState.ServiceStartupTimestamp = fakeStartupTimestamp;
        _server.ApplicationState.LastSuccessfulExport = DateTimeOffset.MinValue;
        
        // Act
        var state = await httpClient.GetFromJsonAsync<TimeTrackApplicationStatePublic>(
            "api/ui/state",
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotNull(state);
        Assert.Equal(fakeStartupTimestamp, state.ServiceStartupTimestamp);
        Assert.Null(state.LastSuccessfulExport);
    }
    
    [Fact]
    public async Task GetCurrentState_with_export()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        var fakeStartupTimestamp = DateTime.UtcNow.AddHours(-2.0);
        var fakeExportTimestamp = fakeStartupTimestamp.AddHours(1.0);
        _server.ApplicationState.ServiceStartupTimestamp = fakeStartupTimestamp;
        _server.ApplicationState.LastSuccessfulExport = fakeExportTimestamp;

        // Act
        var state = await httpClient.GetFromJsonAsync<TimeTrackApplicationStatePublic>(
            "api/ui/state",
            TestContext.Current.CancellationToken);
        
        // Assert
        Assert.NotNull(state);
        Assert.Equal(fakeStartupTimestamp, state.ServiceStartupTimestamp);
        Assert.Equal(fakeExportTimestamp, state.LastSuccessfulExport);
    }
}