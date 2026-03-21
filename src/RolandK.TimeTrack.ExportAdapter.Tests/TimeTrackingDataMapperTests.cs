using RolandK.TimeTrack.Application.Models;
using Xunit;

namespace RolandK.TimeTrack.ExportAdapter.Tests;

[Trait("Category", "NoDependencies")]
public class TimeTrackingDataMapperTests
{
    [Fact]
    public void Create_default_row()
    {
        // Arrange
        var date = new DateOnly(2026, 02, 23);
        var timeTrackingDay = new TimeTrackingDay(
            date,
            TimeTrackingDayType.WorkingDay,
            [new TimeTrackingEntry(
                new TimeTrackingTopicReference("TestCategory", "TestName"),
                2.0,
                1.0,
                TimeTrackingBillingMultiplier.Default,
                TimeTrackingEntryType.Default,
                "Some dummy work")]);
        
        // Act
        var exportedRow = TimeTrackingDataMapper.MapData(
            [timeTrackingDay], date, 1);
        
        // Assert
        Assert.NotNull(exportedRow);
        Assert.Equal("TestCategory", exportedRow[0].Kategorie);
        Assert.Equal("TestName", exportedRow[0].Thema);
        Assert.Equal("2026-02-23", exportedRow[0].Datum);
        Assert.Equal("", exportedRow[0].TagTyp);
        Assert.Equal("", exportedRow[0].ZeilenTyp);
        Assert.Equal(2, exportedRow[0].Zeitaufwand);
        Assert.Equal(1, exportedRow[0].Abgerechnet);
        Assert.Equal(1, exportedRow[0].BillingMultiplier);
        Assert.Equal("Some dummy work", exportedRow[0].Kommentar);
    }
    
    [Fact]
    public void Create_row_with_different_billing_multiplier()
    {
        // Arrange
        var date = new DateOnly(2026, 02, 23);
        var timeTrackingDay = new TimeTrackingDay(
            date,
            TimeTrackingDayType.WorkingDay,
            [new TimeTrackingEntry(
                new TimeTrackingTopicReference("TestCategory", "TestName"),
                2.0,
                1.0,
                1.5,
                TimeTrackingEntryType.Training,
                "Some dummy work")]);
        
        // Act
        var exportedRow = TimeTrackingDataMapper.MapData(
            [timeTrackingDay], date, 1);
        
        // Assert
        Assert.NotNull(exportedRow);
        Assert.Equal("TestCategory", exportedRow[0].Kategorie);
        Assert.Equal("TestName", exportedRow[0].Thema);
        Assert.Equal("2026-02-23", exportedRow[0].Datum);
        Assert.Equal("", exportedRow[0].TagTyp);
        Assert.Equal("S", exportedRow[0].ZeilenTyp);
        Assert.Equal(2, exportedRow[0].Zeitaufwand);
        Assert.Equal(1, exportedRow[0].Abgerechnet);
        Assert.Equal(1.5, exportedRow[0].BillingMultiplier);
        Assert.Equal("Some dummy work", exportedRow[0].Kommentar);
    }
    
    [Fact]
    public void Create_rows_for_empty_days()
    {
        // Arrange
        var date = new DateOnly(2026, 02, 23);
        var timeTrackingDay = new TimeTrackingDay(
            date,
            TimeTrackingDayType.Holiday,
            []);
        
        // Act
        var exportedRow = TimeTrackingDataMapper.MapData(
            [timeTrackingDay], date, 1);
        
        // Assert
        Assert.NotNull(exportedRow);
        Assert.Equal("2026-02-23", exportedRow[0].Datum);
        Assert.Equal("UT", exportedRow[0].TagTyp);
        Assert.Equal(0, exportedRow[0].Zeitaufwand);
        Assert.Equal(0, exportedRow[0].Abgerechnet);
        Assert.Equal(1, exportedRow[0].BillingMultiplier);
    }

    [Fact]
    public void Do_not_export_future_days()
    {
        // Arrange
        var today = new DateOnly(2026, 02, 23);
        var timeTrackingDay = new TimeTrackingDay(
            today.AddDays(1),
            TimeTrackingDayType.Holiday,
            []);
        
        // Act
        var exportedRow = TimeTrackingDataMapper.MapData(
            [timeTrackingDay], today, 1);
        
        // Assert
        Assert.NotNull(exportedRow);
        Assert.Empty(exportedRow);
    }
}