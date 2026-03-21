using Microsoft.Playwright;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class TimeTrackingPageTests
{
    private readonly WebHostServerFixture _server;
    
    public TimeTrackingPageTests(
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
    public async Task Initial_page_should_show_current_day_icon()
    {
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        
        // Assert
        await playwrightSession
            .Expect(playwrightSession.Page.GetByTestId("icon-home"))
            .ToBeVisibleAsync();
    }
    
    [Fact]
    public async Task Initial_page_should_be_on_current_day()
    {
        // Arrange
        var currentDate = _server.MockedStartTimestamp;
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        
        // Assert
        var dateString = $"{currentDate:yyyy-MM-dd}";
        await playwrightSession
            .Expect(playwrightSession.Page.GetByText(dateString, new PageGetByTextOptions(){ Exact = false}))
            .ToBeVisibleAsync();

        var weekDay = currentDate.DayOfWeek.ToString();
        await playwrightSession
            .Expect(playwrightSession.Page.GetByText(weekDay, new PageGetByTextOptions(){ Exact = false}))
            .ToBeVisibleAsync();
    }
    
    [Fact]
    public async Task Initial_connection_state_displayed_connected()
    {
        // Arrange
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        
        // Assert
        await playwrightSession
            .Expect(playwrightSession.Page.GetByText("Connected"))
            .ToBeVisibleAsync();
    }
    
    [Fact]
    public async Task Initial_connection_state_displayed_not_connected()
    {
        // Arrange
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        await playwrightSession.Page.RouteAsync(
            url => url.EndsWith("/ui/state"), 
            route => route.FulfillAsync(new RouteFulfillOptions()
            {
                Status = 404,
                ContentType = "text/plain",
                Body = "Mocked error"
            }));
        
        // Assert
        await playwrightSession
            .Expect(playwrightSession.Page.GetByText("Not connected"))
            .ToBeVisibleAsync();
    }
    
    [Fact]
    public async Task Initial_connection_state_tooltip_to_be_displayed()
    {
        // Arrange
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();

        // Act
        await playwrightSession.Page.GetByText("Connected").HoverAsync();
        
        // Assert
        await playwrightSession
            .Expect(playwrightSession.Page.GetByText("Startup timestamp:"))
            .ToBeVisibleAsync();
    }
}