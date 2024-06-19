namespace RKTimeTrack.Application.Models;

public class TimeTrackingYearMetadata(int maxWeekNumber)
{
    public int MaxWeekNumber { get; } = maxWeekNumber;
}