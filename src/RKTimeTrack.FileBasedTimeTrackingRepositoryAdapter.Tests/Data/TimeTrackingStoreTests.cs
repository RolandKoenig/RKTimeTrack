using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using RKTimeTrack.Application.Models;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Tests.Data;

public class TimeTrackingStoreTests
{
    [Fact]
    public void AddDaysInCorrectOrder()
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);
        
        // Act
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 1), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 2), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 3), 
            TimeTrackingDayType.WorkingDay,
            Array.Empty<TimeTrackingEntry>()));
        
        // Assert
        sut.Store.Count.Should().Be(3);
        sut.Store[0].Date.Should().Be(new DateOnly(2022, 1, 1));
        sut.Store[0].Type.Should().Be(TimeTrackingDayType.Weekend);
        sut.Store[1].Date.Should().Be(new DateOnly(2022, 1, 2));
        sut.Store[1].Type.Should().Be(TimeTrackingDayType.Weekend);
        sut.Store[2].Date.Should().Be(new DateOnly(2022, 1, 3));
        sut.Store[2].Type.Should().Be(TimeTrackingDayType.WorkingDay);
    }
    
    [Fact]
    public void AddDaysInWrongOrder()
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);
        
        // Act
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 3), 
            TimeTrackingDayType.WorkingDay,
            Array.Empty<TimeTrackingEntry>()));
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 1), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 2), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        
        // Assert
        sut.Store.Count.Should().Be(3);
        sut.Store[0].Date.Should().Be(new DateOnly(2022, 1, 1));
        sut.Store[0].Type.Should().Be(TimeTrackingDayType.Weekend);
        sut.Store[1].Date.Should().Be(new DateOnly(2022, 1, 2));
        sut.Store[1].Type.Should().Be(TimeTrackingDayType.Weekend);
        sut.Store[2].Date.Should().Be(new DateOnly(2022, 1, 3));
        sut.Store[2].Type.Should().Be(TimeTrackingDayType.WorkingDay);
    }

    [Fact]
    public void GetWeek_which_is_not_existing()
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);

        // Act
        var week = sut.GetOrCreateWeek(2024, 25);
        
        // Assert
        sut.Store.Count.Should().Be(7);
        week.Year.Should().Be(2024);
        week.WeekNumber.Should().Be(25);
        week.Monday.Should().Be(sut.Store[0]);
        week.Monday.Date.Should().Be(new DateOnly(2024, 6, 17));
        week.Monday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Tuesday.Should().Be(sut.Store[1]);
        week.Tuesday.Date.Should().Be(new DateOnly(2024, 6, 18));
        week.Tuesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Wednesday.Should().Be(sut.Store[2]);
        week.Wednesday.Date.Should().Be(new DateOnly(2024, 6, 19));
        week.Wednesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Thursday.Should().Be(sut.Store[3]);
        week.Thursday.Date.Should().Be(new DateOnly(2024, 6, 20));
        week.Thursday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Friday.Should().Be(sut.Store[4]);
        week.Friday.Date.Should().Be(new DateOnly(2024, 6, 21));
        week.Friday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Saturday.Should().Be(sut.Store[5]);
        week.Saturday.Date.Should().Be(new DateOnly(2024, 6, 22));
        week.Saturday.Type.Should().Be(TimeTrackingDayType.Weekend);
        week.Sunday.Should().Be(sut.Store[6]);
        week.Sunday.Date.Should().Be(new DateOnly(2024, 6, 23));
        week.Sunday.Type.Should().Be(TimeTrackingDayType.Weekend);
    }
    
    [Fact]
    public void GetWeek_where_some_days_exist()
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);

        // Act
        var createdFriday = sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2024, 6, 21), 
            TimeTrackingDayType.WorkingDay,
            Array.Empty<TimeTrackingEntry>()));
        var createdTuesday = sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2024, 6, 18), 
            TimeTrackingDayType.WorkingDay,
            Array.Empty<TimeTrackingEntry>()));
        var createdSunday = sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2024, 6, 23), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        var week = sut.GetOrCreateWeek(2024, 25);
        
        // Assert
        sut.Store.Count.Should().Be(7);
        week.Year.Should().Be(2024);
        week.WeekNumber.Should().Be(25);
        week.Monday.Should().Be(sut.Store[0]);
        week.Monday.Date.Should().Be(new DateOnly(2024, 6, 17));
        week.Monday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Tuesday.Should().Be(sut.Store[1]);
        week.Tuesday.Should().Be(createdTuesday);
        week.Tuesday.Date.Should().Be(new DateOnly(2024, 6, 18));
        week.Tuesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Wednesday.Should().Be(sut.Store[2]);
        week.Wednesday.Date.Should().Be(new DateOnly(2024, 6, 19));
        week.Wednesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Thursday.Should().Be(sut.Store[3]);
        week.Thursday.Date.Should().Be(new DateOnly(2024, 6, 20));
        week.Thursday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Friday.Should().Be(sut.Store[4]);
        week.Friday.Should().Be(createdFriday);
        week.Friday.Date.Should().Be(new DateOnly(2024, 6, 21));
        week.Friday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Saturday.Should().Be(sut.Store[5]);
        week.Saturday.Date.Should().Be(new DateOnly(2024, 6, 22));
        week.Saturday.Type.Should().Be(TimeTrackingDayType.Weekend);
        week.Sunday.Should().Be(sut.Store[6]);
        week.Sunday.Should().Be(createdSunday);
        week.Sunday.Date.Should().Be(new DateOnly(2024, 6, 23));
        week.Sunday.Type.Should().Be(TimeTrackingDayType.Weekend);
    }
    
    [Fact]
    public void GetWeek_where_all_days_exist()
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);

        // Act
        var allDays = Enumerable.Range(0, 7).Select(x =>
            sut.AddOrUpdateDay(new TimeTrackingDay(
                new DateOnly(2024, 6, 17 + x), 
                x < 5 ? TimeTrackingDayType.WorkingDay : TimeTrackingDayType.Weekend,
                Array.Empty<TimeTrackingEntry>()))).ToArray();
        var week = sut.GetOrCreateWeek(2024, 25);
        
        // Assert
        sut.Store.Count.Should().Be(7);
        week.Year.Should().Be(2024);
        week.WeekNumber.Should().Be(25);
        week.Monday.Should().Be(sut.Store[0]);
        week.Monday.Should().Be(allDays[0]);
        week.Monday.Date.Should().Be(new DateOnly(2024, 6, 17));
        week.Monday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Tuesday.Should().Be(sut.Store[1]);
        week.Tuesday.Should().Be(allDays[1]);
        week.Tuesday.Date.Should().Be(new DateOnly(2024, 6, 18));
        week.Tuesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Wednesday.Should().Be(sut.Store[2]);
        week.Wednesday.Should().Be(allDays[2]);
        week.Wednesday.Date.Should().Be(new DateOnly(2024, 6, 19));
        week.Wednesday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Thursday.Should().Be(sut.Store[3]);
        week.Thursday.Should().Be(allDays[3]);
        week.Thursday.Date.Should().Be(new DateOnly(2024, 6, 20));
        week.Thursday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Friday.Should().Be(sut.Store[4]);
        week.Friday.Should().Be(allDays[4]);
        week.Friday.Date.Should().Be(new DateOnly(2024, 6, 21));
        week.Friday.Type.Should().Be(TimeTrackingDayType.WorkingDay);
        week.Saturday.Should().Be(sut.Store[5]);
        week.Saturday.Should().Be(allDays[5]);
        week.Saturday.Date.Should().Be(new DateOnly(2024, 6, 22));
        week.Saturday.Type.Should().Be(TimeTrackingDayType.Weekend);
        week.Sunday.Should().Be(sut.Store[6]);
        week.Sunday.Should().Be(allDays[6]);
        week.Sunday.Date.Should().Be(new DateOnly(2024, 6, 23));
        week.Sunday.Type.Should().Be(TimeTrackingDayType.Weekend);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    [InlineData(300)]
    [InlineData(400)]
    [InlineData(500)]
    public void AddOrUpdateDay_and_GetDay_in_fully_random_order(int seed)
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);
        
        var random = new Random(seed);
        var controlDictionary = new Dictionary<DateOnly, TimeTrackingDay>();
        
        // Act
        for (var loop = 0; loop < 1000; loop++)
        {
            var newDate = new DateOnly(2022, 1, 1).AddDays(random.Next(10, 1500));
            var dayType = newDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => TimeTrackingDayType.Weekend,
                DayOfWeek.Sunday => TimeTrackingDayType.Weekend,
                _ => TimeTrackingDayType.WorkingDay
            };

            var newDay = new TimeTrackingDay(
                newDate,
                dayType,
                Array.Empty<TimeTrackingEntry>());
            sut.AddOrUpdateDay(newDay);
            controlDictionary[newDate] = newDay;
        }
        
        // Assert
        sut.Store.Count.Should().Be(controlDictionary.Count);
        foreach (var day in sut.Store)
        {
            controlDictionary.ContainsKey(day.Date).Should().BeTrue();
            controlDictionary[day.Date].Should().Be(day);
        }
        foreach (var actControlDictionaryEntry in controlDictionary)
        {
            var actDayFromStore = sut.GetOrCreateDay(actControlDictionaryEntry.Key);
            actDayFromStore.Should().Be(actControlDictionaryEntry.Value);
        }
    }
    
    [Fact]
    public void EachUpdate_increases_LastChangeTimestamp()
    {
        // Arrange
        var startTimestamp = new DateTimeOffset(2024, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(startTimestamp);
        var sut = new TimeTrackingStore(timeProvider);
        
        // Act + Assert
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 1), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        sut.LastChangeTimestamp.Should().Be(startTimestamp);
        
        var newTimestamp = startTimestamp.AddHours(1.5);
        timeProvider.SetUtcNow(newTimestamp);
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 2), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        sut.LastChangeTimestamp.Should().Be(newTimestamp);
    }
}