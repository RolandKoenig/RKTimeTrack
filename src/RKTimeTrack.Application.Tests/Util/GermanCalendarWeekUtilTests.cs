using FluentAssertions;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.Tests.Util;

public class GermanCalendarWeekUtilTests
{
    [Theory]
    [InlineData(2024, 1, 1, 1, false)]
    [InlineData(2024, 12, 29, 52, false)]
    [InlineData(2024, 12, 30, 1, true)]
    [InlineData(2024, 12, 31, 1, true)]
    [InlineData(2025, 1, 1, 1, false)]
    public void GetCalendarWeek(int year, int month, int day, int expectedWeek, bool expectInNextYear)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var actualWeek = GermanCalendarWeekUtil.GetCalendarWeek(date, out var nextYear);

        // Assert
        actualWeek.Should().Be(expectedWeek);
        nextYear.Should().Be(expectInNextYear);
    }

    [Fact]
    public void GetCalendarWeek_WeekAlwaysInRange()
    {
        // Arrange
        var startDate = new DateOnly(2000, 1, 1);
        var endDate = new DateOnly(2100, 1, 1);
        
        // Act & Assert
        var currentDate = startDate;
        while (currentDate <= endDate)
        {
            var currentCalendarWeek = GermanCalendarWeekUtil.GetCalendarWeek(currentDate, out var _);
            currentCalendarWeek.Should().BeGreaterOrEqualTo(1, because: $"wrong for date {currentDate}");
            currentCalendarWeek.Should().BeLessOrEqualTo(53, because: $"wrong for date {currentDate}");

            currentDate = currentDate.AddDays(1);
        }
    }

    [Theory]
    [InlineData(2024, 19, 2024, 5, 6)]
    [InlineData(2007, 1, 2007, 1, 1)]
    [InlineData(2007, 52, 2007, 12, 24)]
    [InlineData(2008, 1, 2007, 12, 31)]
    [InlineData(2022, 1, 2022, 1, 3)]
    [InlineData(2022, 52, 2022, 12, 26)]
    [InlineData(2023, 1, 2023, 1, 2)]
    [InlineData(2023, 52, 2023, 12, 25)]
    [InlineData(2024, 1, 2024, 1, 1)]
    [InlineData(2024, 52, 2024, 12, 23)]
    [InlineData(2025, 1, 2024, 12, 30)]
    public void GetDateOfMonday(int year, int weekNumber, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Act
        var actualDate = GermanCalendarWeekUtil.GetDateOfMonday(year, weekNumber);

        // Assert
        actualDate.Year.Should().Be(expectedYear);
        actualDate.Month.Should().Be(expectedMonth);
        actualDate.Day.Should().Be(expectedDay);
        actualDate.DayOfWeek.Should().Be(DayOfWeek.Monday);
    }
}