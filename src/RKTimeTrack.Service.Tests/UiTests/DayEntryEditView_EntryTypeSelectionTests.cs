using Microsoft.Playwright;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
public class DayEntryEditView_EntryTypeSelectionTests
{
    private readonly WebHostServerFixture _server;
    
    public DayEntryEditView_EntryTypeSelectionTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        PlaywrightUtil.EnsureBrowsersInstalled();
        
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;

        _server.Reset();
    }

    [Theory]
    [InlineData("OnCall", "icon-on-call")]
    [InlineData("Training", "icon-training")]
    public async Task Changing_EntryType_updates_row_image(string entryTypeDisplayName, string expectedIconTestId)
    {
        // Act
        await using var playwright = await _server.StartPlaywrightSessionOnRootPageAsync();
        
        await playwright.Page
            .GetByRole(AriaRole.Button, new() { Name = "New Entry" })
            .ClickAsync();
        
        await playwright.Page
            .Locator("#selected-entry-entrytype")
            .GetByRole(AriaRole.Combobox)
            .ClickAsync();
        
        await playwright.Page
            .Locator("#selected-entry-entrytype_list")
            .GetByLabel(entryTypeDisplayName)
            .ClickAsync();
        
        // Assert
        await playwright
            .Expect(playwright.Page.GetByTestId(expectedIconTestId))
            .ToBeVisibleAsync();
    }
}