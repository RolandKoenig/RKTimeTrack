namespace RKTimeTrack.Application.Models;

public class TimeTrackingDay
{
    public DateOnly Date { get; set; }
    
    public TimeTrackingTopic Topic { get; set; }
    
    public TimeTrackingDayType Type { get; set; }

    public List<TimeTrackingRow> Entries { get; set; } = new();
}