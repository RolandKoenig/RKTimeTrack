using System.Net.Http.Json;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

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
        Assert.NotNull(week);
        Assert.Contains(week!.GetAllDays(), x => x.Date == DateOnly.FromDateTime(startDate.DateTime));
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
        Assert.NotNull(week);
        
        Assert.NotNull(week!.Monday);
        Assert.Equal(new DateOnly(2024, 12, 16), week.Monday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Monday.Type);
        Assert.Empty(week.Monday.Entries);
        
        Assert.NotNull(week.Tuesday);
        Assert.Equal(new DateOnly(2024, 12, 17), week.Tuesday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Tuesday.Type);
        Assert.Empty(week.Tuesday.Entries);
        
        Assert.NotNull(week.Wednesday);
        Assert.Equal(new DateOnly(2024, 12, 18), week.Wednesday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Wednesday.Type);
        Assert.Empty(week.Wednesday.Entries);
        
        Assert.NotNull(week.Thursday);
        Assert.Equal(new DateOnly(2024, 12, 19), week.Thursday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Thursday.Type);
        Assert.Empty(week.Thursday.Entries);
        
        Assert.NotNull(week.Friday);
        Assert.Equal(new DateOnly(2024, 12, 20), week.Friday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Friday.Type);
        Assert.Empty(week.Friday.Entries);
        
        Assert.NotNull(week.Saturday);
        Assert.Equal(new DateOnly(2024, 12, 21), week.Saturday.Date);
        Assert.Equal(TimeTrackingDayType.Weekend, week.Saturday.Type);
        Assert.Empty(week.Saturday.Entries);
        
        Assert.NotNull(week.Sunday);
        Assert.Equal(new DateOnly(2024, 12, 22), week.Sunday.Date);
        Assert.Equal(TimeTrackingDayType.Weekend, week.Sunday.Type);
        Assert.Empty(week.Sunday.Entries);
    }
}