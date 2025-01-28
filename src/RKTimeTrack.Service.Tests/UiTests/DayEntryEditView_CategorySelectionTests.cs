using Microsoft.Playwright;
using NSubstitute;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
public class DayEntryEditView_CategorySelectionTests
{
    private readonly WebHostServerFixture _server;
    
    public DayEntryEditView_CategorySelectionTests(
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
    public async Task PossibleTopicCategories_single_default_category()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                    new TimeTrackingTopic("TestCategory1", "Topic1")
                ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicCategories(playwrightSession.Page);
        
        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("TestCategory1", possibleOptions[0]);
    }
    
    [Fact]
    public async Task PossibleTopicCategories_with_category_valid_for_current_date()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory2", "Topic2", 
                    startDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(-1),
                    endDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(1))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicCategories(playwrightSession.Page);
        
        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Contains("TestCategory1", possibleOptions);
        Assert.Contains("TestCategory2", possibleOptions);
    }
    
    [Fact]
    public async Task PossibleTopicCategories_with_category_with_EndDate_at_today()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory2", "Topic2", 
                    endDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicCategories(playwrightSession.Page);
        
        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Contains("TestCategory1", possibleOptions);
        Assert.Contains("TestCategory2", possibleOptions);
    }
    
    [Fact]
    public async Task PossibleTopicCategories_with_category_with_StartDate_at_today()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory2", "Topic2", 
                    startDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicCategories(playwrightSession.Page);
        
        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Contains("TestCategory1", possibleOptions);
        Assert.Contains("TestCategory2", possibleOptions);
    }
    
    [Fact]
    public async Task PossibleTopicCategories_with_filtered_category_because_of_EndDate()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory2", "Topic2", 
                    endDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(-1))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicCategories(playwrightSession.Page);
        
        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("TestCategory1", possibleOptions[0]);
    }

    [Fact]
    public async Task PossibleTopicCategories_with_filtered_category_because_of_StartDate()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory2", "Topic2", 
                    startDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(1))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicCategories(playwrightSession.Page);
        
        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("TestCategory1", possibleOptions[0]);
    }
    
    /// <summary>
    /// Gets all possible options from the 'Category' Select component.
    /// </summary>
    private static async Task<IReadOnlyList<string>> GetShownTopicCategories(IPage page)
    {
        // Create a new entry to see the DayEntryEditView
        await page.GetByText("New Entry").ClickAsync();
        
        // Open the Select component for the category
        await page
            .Locator("#selected-entry-topic-category")
            .GetByRole(AriaRole.Combobox)
            .ClickAsync();

        // Get all entries in the Select component
        return await page
            .Locator("#selected-entry-topic-category_list > li")
            .AllInnerTextsAsync();
    }
}