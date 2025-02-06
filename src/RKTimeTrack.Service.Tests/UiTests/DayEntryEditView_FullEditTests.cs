using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using NSubstitute;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.Application.Util;
using RKTimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.Tests.UiTests;

[Collection(nameof(TestEnvironmentCollection))]
public class DayEntryEditView_FullEditTests
{
    private readonly WebHostServerFixture _server;
    
    public DayEntryEditView_FullEditTests(
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
    public async Task FullEdit_NotInvoiced_Entry()
    {
        // ##### Arrange
        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<TimeTrackingTopic>>([
                new TimeTrackingTopic("TestCategory1", "Topic1")
            ]));
        
        // ###### Act
        await using var playwright = await _server.StartPlaywrightSessionOnRootPageAsync();
        var page = playwright.Page;
        
        await page.GetByText("New Entry").ClickAsync();
        
        // Topic category
        await page.Locator("#selected-entry-topic-category").GetByRole(AriaRole.Combobox).ClickAsync();
        await page.GetByLabel("TestCategory1").ClickAsync();
        
        // Topic name
        await page.Locator("#selected-entry-topic-name").GetByRole(AriaRole.Combobox).ClickAsync();
        await page.GetByLabel("Topic1").ClickAsync();

        // Effort (SpinButton)
        await page.Locator("#current-row-effort button").First.ClickAsync();
        
        // Description
        await page.GetByLabel("Description").FillAsync("Test-Description");
        
        // ##### Assert
        await TestUtil.TryXTimesAsync(
            async () =>
            {
                var storedWeekData = await GetCurrentWeekFromRepositoryAsync(
                    _server.MockedStartTimestamp, CancellationToken.None);
                var dayToCheck = storedWeekData
                    .GetAllDays()
                    .First(x => x.Date == DateOnly.FromDateTime(_server.MockedStartTimestamp.DateTime));
            
                Assert.Single(dayToCheck.Entries);
                Assert.Equal("TestCategory1", dayToCheck.Entries[0].Topic.Category);
                Assert.Equal("Topic1", dayToCheck.Entries[0].Topic.Name);
                Assert.Equal(0.25, dayToCheck.Entries[0].EffortInHours.Hours);
                Assert.Equal("Test-Description", dayToCheck.Entries[0].Description);
            }, 
            times: 200,
            delay: TimeSpan.FromMilliseconds(10));
    }

    private async Task<TimeTrackingWeek> GetCurrentWeekFromRepositoryAsync(
        DateTimeOffset date,
        CancellationToken cancellationToken)
    {
        var repository = _server.Services.GetRequiredService<ITimeTrackingRepository>();

        var year = _server.MockedStartTimestamp.Year;
        var week = GermanCalendarWeekUtil.GetCalendarWeek(
            DateOnly.FromDateTime(date.DateTime),
            out var nextYear);
        if (nextYear)
        {
            year++;
        }

        return await repository.GetWeekAsync(year, week, cancellationToken);
    }
}