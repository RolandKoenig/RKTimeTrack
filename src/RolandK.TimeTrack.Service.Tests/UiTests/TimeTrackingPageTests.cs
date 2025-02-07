using Microsoft.Playwright;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RolandK.TimeTrack.Service.Tests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
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
}