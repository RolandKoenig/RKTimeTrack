using Microsoft.Playwright;
using NSubstitute;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
public class DayEntryEditView_NameSelectionTests
{
    private readonly WebHostServerFixture _server;
    
    public DayEntryEditView_NameSelectionTests(
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
    public async Task PossibleTopicNames_single_default_category()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1")
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNames(
            playwrightSession.Page, "TestCategory1");
        
        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("Topic1", possibleOptions[0]);
    }
    
    /// <summary>
    /// Gets all possible options from the 'Name' Select component.
    /// </summary>
    private static async Task<IReadOnlyList<string>> GetShownTopicNames(
        IPage page, string categoryName)
    {
        // Create a new entry to see the DayEntryEditView
        await page.GetByText("New Entry").ClickAsync();
        
        // Select a category
        await page
            .Locator("#selected-entry-topic-category")
            .GetByRole(AriaRole.Combobox)
            .ClickAsync();
        await page.GetByLabel(categoryName).ClickAsync();
        
        // Open the Select component for topic name
        await page
            .Locator("#selected-entry-topic-name")
            .GetByRole(AriaRole.Combobox)
            .ClickAsync();
        
        // Get all entries in the Select component
        return await page
            .Locator("#selected-entry-topic-name_list > li")
            .AllInnerTextsAsync();
    }
}