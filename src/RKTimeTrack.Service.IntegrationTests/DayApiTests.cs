using System.Net.Http.Json;
using FluentAssertions;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.UseCases;
using RKTimeTrack.Service.IntegrationTests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class DayApiTests
{
    private readonly WebHostServerFixture _server;
    
    public DayApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }
    
    [Fact]
    public async Task UpdateDay_FullDataSet()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name6 (With Budget)"),
                        4f,
                        2f,
                        TimeTrackingEntryType.Default,
                        "My dummy description")
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Date.Should().Be(new DateOnly(2024, 12, 17));
        week.Tuesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Tuesday.Entries.Should().HaveCount(1);
        week.Tuesday.Entries[0].Topic.Category.Should().Be("Category1");
        week.Tuesday.Entries[0].Topic.Name.Should().Be("Name6 (With Budget)");
        week.Tuesday.Entries[0].EffortInHours.Hours.Should().Be(4);
        week.Tuesday.Entries[0].EffortBilled.Hours.Should().Be(2);
        week.Tuesday.Entries[0].Type.Should().Be(TimeTrackingEntryType.Default);
        week.Tuesday.Entries[0].Description.Should().Be("My dummy description");
    }
    
    [Theory]
    [InlineData(TimeTrackingDayType.WorkingDay)]
    [InlineData(TimeTrackingDayType.Holiday)]
    [InlineData(TimeTrackingDayType.Weekend)]
    [InlineData(TimeTrackingDayType.CompensatoryTimeOff)]
    [InlineData(TimeTrackingDayType.Ill)]
    [InlineData(TimeTrackingDayType.Training)]
    [InlineData(TimeTrackingDayType.OwnEducation)]
    [InlineData(TimeTrackingDayType.PublicHoliday)]
    public async Task UpdateDay_Property_DayType(TimeTrackingDayType dayType)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                dayType,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name1"),
                        2f)
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Type.Should().Be(dayType);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("Category1", "")]
    [InlineData("Category1", "Name1")]
    public async Task UpdateDay_Property_TopicReference(string topicCategory, string topicName)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference(topicCategory, topicName),
                        2f)
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Entries[0].Topic.Category.Should().Be(topicCategory);
        week.Tuesday.Entries[0].Topic.Name.Should().Be(topicName);
    }
    
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.25)]
    [InlineData(0.5)]
    [InlineData(0.75)]
    [InlineData(2.0)]
    public async Task UpdateDay_Property_EffortInHours(double effortInHours)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name1"),
                        effortInHours)
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Entries[0].EffortInHours.Hours.Should().Be(effortInHours);
    }
    
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.25)]
    [InlineData(0.5)]
    [InlineData(0.75)]
    [InlineData(2.0)]
    public async Task UpdateDay_Property_EffortBilled(double effortBilled)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name1"),
                        effortBilled,
                        effortBilled)
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Entries[0].EffortBilled.Hours.Should().Be(effortBilled);
    }
    
    [Theory]
    [InlineData(TimeTrackingEntryType.Default)]
    [InlineData(TimeTrackingEntryType.Training)]
    [InlineData(TimeTrackingEntryType.OnCall)]
    public async Task UpdateDay_Property_EntryType(TimeTrackingEntryType entryType)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name1"),
                        0f,
                        type: entryType)
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Entries[0].Type.Should().Be(entryType);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("Some dummy description")]
    public async Task UpdateDay_Property_Description(string description)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        await httpClient.PostAsJsonAsync(
            "api/ui/day",
            new UpdateDay_Request(
                new DateOnly(2024, 12, 17),
                TimeTrackingDayType.WorkingDay,
                [
                    new TimeTrackingEntry(
                        new TimeTrackingTopicReference("Category1", "Name1"),
                        0f,
                        description: description)
                ]));
        
        // Assert
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        week.Should().NotBeNull();

        week!.Tuesday.Should().NotBeNull();
        week.Tuesday.Entries[0].Description.Should().Be(description);
    }
}