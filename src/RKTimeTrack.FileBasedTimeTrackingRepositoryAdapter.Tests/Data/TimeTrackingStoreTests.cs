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
        Assert.Equal(3, sut.Store.Count);
        Assert.Equal(new DateOnly(2022, 1, 1), sut.Store[0].Date);
        Assert.Equal(TimeTrackingDayType.Weekend, sut.Store[0].Type);
        Assert.Equal(new DateOnly(2022, 1, 2), sut.Store[1].Date);
        Assert.Equal(TimeTrackingDayType.Weekend, sut.Store[1].Type);
        Assert.Equal(new DateOnly(2022, 1, 3), sut.Store[2].Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, sut.Store[2].Type);
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
        Assert.Equal(3, sut.Store.Count);
        Assert.Equal(new DateOnly(2022, 1, 1), sut.Store[0].Date);
        Assert.Equal(TimeTrackingDayType.Weekend, sut.Store[0].Type);
        Assert.Equal(new DateOnly(2022, 1, 2), sut.Store[1].Date);
        Assert.Equal(TimeTrackingDayType.Weekend, sut.Store[1].Type);
        Assert.Equal(new DateOnly(2022, 1, 3), sut.Store[2].Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, sut.Store[2].Type);
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
        Assert.Equal(7, sut.Store.Count);
        Assert.Equal(2024, week.Year);
        Assert.Equal(25, week.WeekNumber);
        Assert.Same(sut.Store[0], week.Monday);
        Assert.Equal(new DateOnly(2024, 6, 17), week.Monday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Monday.Type);
        Assert.Same(sut.Store[1], week.Tuesday);
        Assert.Equal(new DateOnly(2024, 6, 18), week.Tuesday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Tuesday.Type);
        Assert.Same(sut.Store[2], week.Wednesday);
        Assert.Equal(new DateOnly(2024, 6, 19), week.Wednesday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Wednesday.Type);
        Assert.Same(sut.Store[3], week.Thursday);
        Assert.Equal(new DateOnly(2024, 6, 20), week.Thursday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Thursday.Type);
        Assert.Same(sut.Store[4], week.Friday);
        Assert.Equal(new DateOnly(2024, 6, 21), week.Friday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Friday.Type);
        Assert.Same(sut.Store[5], week.Saturday);
        Assert.Equal(new DateOnly(2024, 6, 22), week.Saturday.Date);
        Assert.Equal(TimeTrackingDayType.Weekend, week.Saturday.Type);
        Assert.Same(sut.Store[6], week.Sunday);
        Assert.Equal(new DateOnly(2024, 6, 23), week.Sunday.Date);
        Assert.Equal(TimeTrackingDayType.Weekend, week.Sunday.Type);
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
        Assert.Equal(7, sut.Store.Count);
        Assert.Equal(2024, week.Year);
        Assert.Equal(25, week.WeekNumber);
        Assert.Same(sut.Store[0], week.Monday);
        Assert.Equal(new DateOnly(2024, 6, 17), week.Monday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Monday.Type);
        Assert.Same(sut.Store[1], week.Tuesday);
        Assert.Same(createdTuesday, week.Tuesday);
        Assert.Equal(new DateOnly(2024, 6, 18), week.Tuesday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Tuesday.Type);
        Assert.Same(sut.Store[2], week.Wednesday);
        Assert.Equal(new DateOnly(2024, 6, 19), week.Wednesday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Wednesday.Type);
        Assert.Same(sut.Store[3], week.Thursday);
        Assert.Equal(new DateOnly(2024, 6, 20), week.Thursday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Thursday.Type);
        Assert.Same(sut.Store[4], week.Friday);
        Assert.Same(createdFriday, week.Friday);
        Assert.Equal(new DateOnly(2024, 6, 21), week.Friday.Date);
        Assert.Equal(TimeTrackingDayType.WorkingDay, week.Friday.Type);
        Assert.Same(sut.Store[5], week.Saturday);
        Assert.Equal(new DateOnly(2024, 6, 22), week.Saturday.Date);
        Assert.Equal(TimeTrackingDayType.Weekend, week.Saturday.Type);
        Assert.Same(sut.Store[6], week.Sunday);
        Assert.Same(createdSunday, week.Sunday);
        Assert.Equal(new DateOnly(2024, 6, 23), week.Sunday.Date);
        Assert.Equal(TimeTrackingDayType.Weekend, week.Sunday.Type);
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
        Assert.Equal(startTimestamp, sut.LastChangeTimestamp);
        
        var newTimestamp = startTimestamp.AddHours(1.5);
        timeProvider.SetUtcNow(newTimestamp);
        sut.AddOrUpdateDay(new TimeTrackingDay(
            new DateOnly(2022, 1, 2), 
            TimeTrackingDayType.Weekend,
            Array.Empty<TimeTrackingEntry>()));
        Assert.Equal(newTimestamp, sut.LastChangeTimestamp);
    }
}