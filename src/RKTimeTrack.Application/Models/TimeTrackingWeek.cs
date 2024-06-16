namespace RKTimeTrack.Application.Models;

public class TimeTrackingWeek
{
    public TimeTrackingDay Monday { get; set; }
    
    public TimeTrackingDay Tuesday { get; set; }
    
    public TimeTrackingDay Wednesday { get; set; }
    
    public TimeTrackingDay Thursday { get; set; }
    
    public TimeTrackingDay Friday { get; set; }
    
    public TimeTrackingDay Saturday { get; set; }
    
    public TimeTrackingDay Sunday { get; set; }
}