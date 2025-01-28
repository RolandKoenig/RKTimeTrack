using FluentAssertions;
using Microsoft.Playwright;
using RKTimeTrack.Service.IntegrationTests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
public class StartupTests
{
    private readonly WebHostServerFixture _server;
    
    public StartupTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        PlaywrightUtil.EnsureBrowsersInstalled();
        
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }
    
    [Fact]
    public async Task Open_Index_Page()
    {
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        
        // Assert
        await playwrightSession
            .Expect(playwrightSession.Page)
            .ToHaveTitleAsync("RK TimeTrack");
    }
}