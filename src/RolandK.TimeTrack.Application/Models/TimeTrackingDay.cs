namespace RolandK.TimeTrack.Application.Models;

public class TimeTrackingDay(DateOnly date, TimeTrackingDayType type, IReadOnlyList<TimeTrackingEntry> entries)
{
    public DateOnly Date { get; } = date;

    public TimeTrackingDayType Type { get; } = type;

    public IReadOnlyList<TimeTrackingEntry> Entries { get; } = entries;
}