namespace RKTimeTrack.Application.Models;

public class TimeTrackingRow
{
    public TimeTrackingTopic TimeTrackingTopic { get; }
    
    public DateOnly Date { get; }
    
    public TimeTrackingDayType TimeTrackingDayType { get; }
    
    public TimeTrackingHours EffortInHours { get; }
    
    public TimeTrackingHours EffortBilled { get; }
    
    public string Description { get; }
}