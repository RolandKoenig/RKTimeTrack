using System.Net.Http.Json;
using FluentAssertions;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Service.IntegrationTests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class ApiTests
{
    private readonly WebHostServerFixture _server;
    
    public ApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        PlaywrightUtil.EnsureBrowsersInstalled();
        
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
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