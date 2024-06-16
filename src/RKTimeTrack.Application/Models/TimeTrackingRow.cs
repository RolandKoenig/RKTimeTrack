namespace RKTimeTrack.Application.Models;

public class TimeTrackingRow
{
    public TimeTrackingTopicReference Topic { get; set; }

    public TimeTrackingHours EffortInHours { get; set; } = 0;

    public TimeTrackingHours EffortBilled { get; set; } = 0;

    public string Description { get; set; } = "";
}