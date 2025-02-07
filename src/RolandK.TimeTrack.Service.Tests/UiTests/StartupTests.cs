using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RolandK.TimeTrack.Service.Tests.UiTests;

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
    public async Task Initial_page_should_have_correct_title()
    {
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        
        // Assert
        await playwrightSession
            .Expect(playwrightSession.Page)
            .ToHaveTitleAsync("RolandK TimeTrack");
    }
}