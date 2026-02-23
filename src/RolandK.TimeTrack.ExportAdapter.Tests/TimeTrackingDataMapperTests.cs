using RolandK.TimeTrack.Application.Models;
using Xunit;

namespace RolandK.TimeTrack.ExportAdapter.Tests;

public class TimeTrackingDataMapperTests
{
    [Fact]
    public void Create_default_row()
    {
        // Arrange
        var timeTrackingDay = new TimeTrackingDay(
            new DateOnly(2026, 02, 23),
            TimeTrackingDayType.WorkingDay,
            [new TimeTrackingEntry(
                new TimeTrackingTopicReference("TestCategory", "TestName"),
                2.0,
                1.0,
                TimeTrackingEntryType.Default,
                "Some dummy work")]);
        
        // Act
        var exportedRow = TimeTrackingDataMapper.MapData(
            [timeTrackingDay], 1);
        
        // Assert
        Assert.NotNull(exportedRow);
        Assert.Equal("TestCategory", exportedRow[0].Kategorie);
        Assert.Equal("TestName", exportedRow[0].Thema);
        Assert.Equal("2026-02-23", exportedRow[0].Datum);
        Assert.Equal("", exportedRow[0].TagTyp);
        Assert.Equal("", exportedRow[0].ZeilenTyp);
        Assert.Equal(2, exportedRow[0].Zeitaufwand);
        Assert.Equal(1, exportedRow[0].Abgerechnet);
        Assert.Equal("Some dummy work", exportedRow[0].Kommentar);
    }
    
    [Fact]
    public void Create_rows_for_empty_days()
    {
        // Arrange
        var timeTrackingDay = new TimeTrackingDay(
            new DateOnly(2026, 02, 23),
            TimeTrackingDayType.Holiday,
            []);
        
        // Act
        var exportedRow = TimeTrackingDataMapper.MapData(
            [timeTrackingDay], 1);
        
        // Assert
        Assert.NotNull(exportedRow);
        Assert.Equal("2026-02-23", exportedRow[0].Datum);
        Assert.Equal("UT", exportedRow[0].TagTyp);
        Assert.Equal(0, exportedRow[0].Zeitaufwand);
        Assert.Equal(0, exportedRow[0].Abgerechnet);
    }
}