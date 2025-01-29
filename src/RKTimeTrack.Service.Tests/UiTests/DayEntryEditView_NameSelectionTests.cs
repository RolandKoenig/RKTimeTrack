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
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("Topic1", possibleOptions[0]);
    }
    
    [Fact]
    public async Task PossibleTopicNames_two_topics_in_category()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic("TestCategory1", "Topic2")
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Equal("Topic1", possibleOptions[0]);
        Assert.Equal("Topic2", possibleOptions[1]);
    }
    
    [Fact]
    public async Task PossibleTopicNames_with_topic_valid_for_current_date()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory1", "Topic2",
                    startDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(-1),
                    endDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(1))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Equal("Topic1", possibleOptions[0]);
        Assert.Equal("Topic2", possibleOptions[1]);
    }
    
    [Fact]
    public async Task PossibleTopicNames_with_topic_with_StartDate_at_today()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory1", "Topic2",
                    startDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Equal("Topic1", possibleOptions[0]);
        Assert.Equal("Topic2", possibleOptions[1]);
    }
    
    [Fact]
    public async Task PossibleTopicNames_with_topic_with_EndDate_at_today()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory1", "Topic2",
                    endDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Equal(2, possibleOptions.Count);
        Assert.Equal("Topic1", possibleOptions[0]);
        Assert.Equal("Topic2", possibleOptions[1]);
    }
    
    [Fact]
    public async Task PossibleTopicNames_with_filtered_topic_name_because_of_StartDate()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory1", "Topic2",
                    startDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(1))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("Topic1", possibleOptions[0]);
    }
    
    [Fact]
    public async Task PossibleTopicNames_with_filtered_topic_name_because_of_EndDate()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1"),
                new TimeTrackingTopic(
                    "TestCategory1", "Topic2",
                    endDate: DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime).AddDays(-1))
            ]));
        
        // Act
        await using var playwrightSession = await _server.StartPlaywrightSessionOnRootPageAsync();
        var possibleOptions = await GetShownTopicNamesAsync(
            playwrightSession.Page, "TestCategory1");

        // Assert
        Assert.Single(possibleOptions);
        Assert.Equal("Topic1", possibleOptions[0]);
    }
    
    /// <summary>
    /// Gets all possible options from the 'Name' Select component.
    /// </summary>
    private static async Task<IReadOnlyList<string>> GetShownTopicNamesAsync(
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