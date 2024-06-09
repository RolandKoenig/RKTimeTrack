namespace RKTimeTrack.Application.Models;

public class TimeTrackingRow
{
    public Project Project { get; }
    
    public DateOnly Date { get; }
    
    public DayType DayType { get; }
    
    public double EffortInHours { get; }
    
    public double EffortBilled { get; }
    
    public string Description { get; }
}