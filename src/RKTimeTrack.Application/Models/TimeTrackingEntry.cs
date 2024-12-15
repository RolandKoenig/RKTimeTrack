namespace RKTimeTrack.Application.Models;

public class TimeTrackingEntry(
    TimeTrackingTopicReference topic, 
    TimeTrackingHours effortInHours, 
    TimeTrackingHours effortBilled = default,
    TimeTrackingEntryType type = default,
    string description = "")
{
    public TimeTrackingTopicReference Topic { get; } = topic;
    
    public TimeTrackingHours EffortInHours { get; } = effortInHours;

    public TimeTrackingHours EffortBilled { get; } = effortBilled;

    public TimeTrackingEntryType Type { get; } = type;

    public string Description { get; } = description;
}