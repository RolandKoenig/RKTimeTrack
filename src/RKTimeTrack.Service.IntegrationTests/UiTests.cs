using FluentAssertions;
using Microsoft.Playwright;
using RKTimeTrack.Service.IntegrationTests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class UiTests
{
    private const bool HEADLESS_MODE = true;
    private static readonly float? SLOW_MODE_MILLISECONDS = null;
    
    private readonly WebHostServerFixture _server;
    
    public UiTests(
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
        // Arrange
        var rootUri = _server.RootUri;
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = HEADLESS_MODE,
            SlowMo = SLOW_MODE_MILLISECONDS
        });
        var page = await browser.NewPageAsync();

        // Act
        await page.GotoAsync(rootUri.AbsoluteUri);

        // Assert
        var title = await page.TitleAsync();
        title.Should().Be("RK TimeTrack");
    }
}