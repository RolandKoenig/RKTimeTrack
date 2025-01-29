using System.Net.Http.Json;
using FluentAssertions;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class WeekApiTests
{
    private readonly WebHostServerFixture _server;
    
    public WeekApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }

    [Theory]
    [InlineData(2024, 12, 1)]
    [InlineData(2024, 12, 30)]
    [InlineData(2024, 12, 31)]
    [InlineData(2025, 1, 1)]
    [InlineData(2025, 6, 1)]
    public async Task GetCurrentWeek_WhichWasNeverAccessedBefore(int year, int month, int day)
    {
        // Arrange
        var startDate = new DateTimeOffset(year, month, day, 8, 0, 0, TimeSpan.Zero);
        _server.TimeProviderMock.Reset(startDate);
        
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week");
        
        // Assert
        week.Should().NotBeNull();
        
        week!.GetAllDays().Should().Contain(x => x.Date == DateOnly.FromDateTime(startDate.DateTime));
    }

    [Fact]
    public async Task GetWeek_WhichWasNeverAccessedBefore()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;
        
        // Act
        var week = await httpClient.GetFromJsonAsync<TimeTrackingWeek>("api/ui/week/2024/51");
        
        // Assert
        week.Should().NotBeNull();
        
        week!.Monday.Should().NotBeNull();
        week.Monday.Date.Should().Be(new DateOnly(2024, 12, 16));
        week.Monday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Monday.Entries.Should().BeEmpty();
        
        week.Tuesday.Should().NotBeNull();
        week.Tuesday.Date.Should().Be(new DateOnly(2024, 12, 17));
        week.Tuesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Tuesday.Entries.Should().BeEmpty();
        
        week.Wednesday.Should().NotBeNull();
        week.Wednesday.Date.Should().Be(new DateOnly(2024, 12, 18));
        week.Wednesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Wednesday.Entries.Should().BeEmpty();
        
        week.Thursday.Should().NotBeNull();
        week.Thursday.Date.Should().Be(new DateOnly(2024, 12, 19));
        week.Thursday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Thursday.Entries.Should().BeEmpty();
        
        week.Friday.Should().NotBeNull();
        week.Friday.Date.Should().Be(new DateOnly(2024, 12, 20));
        week.Friday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Friday.Entries.Should().BeEmpty();
        
        week.Saturday.Should().NotBeNull();
        week.Saturday.Date.Should().Be(new DateOnly(2024, 12, 21));
        week.Saturday.Type.Should().Be(TimeTrackingDayType.Weekend);
        week.Saturday.Entries.Should().BeEmpty();
        
        week.Sunday.Should().NotBeNull();
        week.Sunday.Date.Should().Be(new DateOnly(2024, 12, 22));
        week.Sunday.Type.Should().Be(TimeTrackingDayType.Weekend);
        week.Sunday.Entries.Should().BeEmpty();
    }
}