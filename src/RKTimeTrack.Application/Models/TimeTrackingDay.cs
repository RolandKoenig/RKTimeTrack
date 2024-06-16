namespace RKTimeTrack.Application.Models;

public class TimeTrackingDay(DateOnly date, TimeTrackingDayType type, IReadOnlyList<TimeTrackingRow> entries)
{
    public DateOnly Date { get; } = date;

    public TimeTrackingDayType Type { get; } = type;

    public IReadOnlyList<TimeTrackingRow> Entries { get; } = entries;
}