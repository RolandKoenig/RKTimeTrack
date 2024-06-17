namespace RKTimeTrack.Application.Models;

public class TimeTrackingRow(TimeTrackingTopicReference topic, TimeTrackingHours effortInHours, TimeTrackingHours effortBilled = default, string description = "")
{
    public TimeTrackingTopicReference Topic { get; } = topic;

    public TimeTrackingHours EffortInHours { get; } = effortInHours;

    public TimeTrackingHours EffortBilled { get; } = effortBilled;

    public string Description { get; } = description;
}