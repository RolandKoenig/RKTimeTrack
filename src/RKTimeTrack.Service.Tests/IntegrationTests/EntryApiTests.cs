using System.Net.Http.Json;
using FluentAssertions;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
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
    public async Task SearchEntries_SingleEntryFound()
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
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]));
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
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]));
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("#4210"));
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>();

        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(1);
        response![0].Description.Should().Be("#4210 My dummy description 2");
    }
    
    [Fact]
    public async Task SearchEntries_NoEntryFound()
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
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]));
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
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]));
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("#9999"));
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>();

        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(0);
    }
    
    [Fact]
    public async Task SearchEntries_MoreEntriesFound()
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
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]));
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
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]));
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("dummy"));
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>();

        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(2);
        response![0].Description.Should().Be("#4210 My dummy description 2");
        response[1].Description.Should().Be("#1023 My dummy description");
    }
    
    [Fact]
    public async Task SearchEntries_MoreEntriesFound_Not_CaseSensitive()
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
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]));
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
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]));
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("DUMMY"));
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>();

        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(2);
        response![0].Description.Should().Be("#4210 My dummy description 2");
        response[1].Description.Should().Be("#1023 My dummy description");
    }
    
    [Fact]
    public async Task SearchEntries_MoreEntriesFound_ButLimitedBy_MaxSearchResults()
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
                        TimeTrackingEntryType.Default,
                        "#1023 My dummy description")
                ]));
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
                        TimeTrackingEntryType.Default,
                        "#4210 My dummy description 2")
                ]));
        
        // Act
        var responseMessage = await httpClient.PostAsJsonAsync(
            "api/ui/entries",
            new SearchEntriesByText_Request("dummy", 1));
        var response = await responseMessage.Content.ReadFromJsonAsync<IReadOnlyList<TimeTrackingEntry>>();

        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(1);
        response![0].Description.Should().Be("#4210 My dummy description 2");
    }
}