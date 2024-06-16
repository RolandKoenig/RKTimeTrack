namespace RKTimeTrack.Application.Models;

public class TimeTrackingRow
{
    public TimeTrackingTopic Topic { get; }
    
    public DateOnly Date { get; }
    
    public TimeTrackingDayType DayType { get; }
    
    public TimeTrackingHours EffortInHours { get; }
    
    public TimeTrackingHours EffortBilled { get; }
    
    public string Description { get; }
}