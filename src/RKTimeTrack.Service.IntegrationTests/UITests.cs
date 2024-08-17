using Microsoft.Playwright;
using RKTimeTrack.Service.IntegrationTests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class UITests
{
    private const bool HEADLESS_MODE = true;
    
    private readonly WebHostServerFixture _server;
    
    public UITests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        PlaywrightUtil.EnsureBrowsersInstalled();
        
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
    }
    
    [Fact]
    public async Task Open_Index_Page()
    {
        // Arrange
        var rootUri = _server.RootUri;
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = HEADLESS_MODE
        });
        var page = await browser.NewPageAsync();

        // Act
        await page.GotoAsync(rootUri.AbsoluteUri);

        // Assert
        var title = await page.TitleAsync();
        Assert.Equal("RK TimeTrack", title);
    }
}