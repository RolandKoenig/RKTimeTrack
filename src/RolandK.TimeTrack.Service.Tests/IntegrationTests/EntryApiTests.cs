using System.Net.Http.Json;
using NSubstitute;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.UseCases;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
[Trait("Category", "NoDependencies")]
public class EntryApiTests
{
    private readonly WebHostServerFixture _server;
    
    public EntryApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }

    [Fact]
    public async Task SearchEntries_ByBilled_SingleEntryFound()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        false,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("", Billed: true),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response);
        Assert.Equal("#1023 My dummy description", response[0].Description);
    }
    
    [Fact]
    public async Task SearchEntries_ByNotBilledAndCanBeInvoiced_SingleEntryFound()
    {
        // Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("Cat1", "Topic1", canBeInvoiced: true)
            ]));
        
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Cat1", "Topic1"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Cat1", "Topic1"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        false,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("", Billed: false, CanBeInvoiced: true),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response);
        Assert.Equal("#4210 My dummy description 2", response[0].Description);
    }
    
    [Fact]
    public async Task SearchEntries_ByText_SingleEntryFound()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("#4210"),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response);
        Assert.Equal("#4210 My dummy description 2", response[0].Description);
    }
    
    [Fact]
    public async Task SearchEntries_ByText_NoEntryFound()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("#9999"),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }
    
    [Fact]
    public async Task SearchEntries_ByText_MoreEntriesFound()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("dummy"),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Count);
        Assert.Equal("#4210 My dummy description 2", response[0].Description);
        Assert.Equal("#1023 My dummy description", response[1].Description);
    }
    
    [Fact]
    public async Task SearchEntries_ByText_MoreEntriesFound_Not_CaseSensitive()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("DUMMY"),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Count);
        Assert.Equal("#4210 My dummy description 2", response[0].Description);
        Assert.Equal("#1023 My dummy description", response[1].Description);
    }
    
    [Fact]
    public async Task SearchEntries_ByText_MoreEntriesFound_ButLimitedBy_MaxSearchResults()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 16),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]),
            TestContext.Current.CancellationToken);
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        1f,
                        1f,
                        TimeTrackingBillingMultiplier.Default,
                        true,
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]),
            TestContext.Current.CancellationToken);
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("dummy", MaxSearchResults: 1),
            TestContext.Current.CancellationToken);
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(response);
        Assert.Single(response);
        Assert.Equal("#4210 My dummy description 2", response[0].Description);
    }
}