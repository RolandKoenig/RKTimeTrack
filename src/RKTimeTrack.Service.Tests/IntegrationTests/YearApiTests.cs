using System.Net.Http.Json;
using FluentAssertions;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class YearApiTests
{
    private readonly WebHostServerFixture _server;
    
    public YearApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }

    [Theory]
    [InlineData(2022, 52)]
    [InlineData(2023, 52)]
    [InlineData(2024, 52)]
    [InlineData(2025, 52)]
    [InlineData(2026, 53)]
    [InlineData(2027, 52)]
    [InlineData(2028, 52)]
    public async Task GetYearMetadata(int year, int expectedWeekCount)
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        // Act
        var yearMetadata = await httpClient.GetFromJsonAsync<TimeTrackingYearMetadata>($"api/ui/year/{year}/metadata");
        
        // Assert
        yearMetadata.Should().NotBeNull();
        yearMetadata!.MaxWeekNumber.Should().Be(expectedWeekCount);
    }
}