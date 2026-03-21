namespace RolandK.TimeTrack.Application.Models;

public class TimeTrackingEntry(
    TimeTrackingTopicReference topic, 
    TimeTrackingHours effortInHours, 
    TimeTrackingHours effortBilled = default,
    TimeTrackingBillingMultiplier billingMultiplier = default,
    TimeTrackingEntryType type = default,
    string description = "")
{
    public TimeTrackingTopicReference Topic { get; } = topic;
    
    public TimeTrackingHours EffortInHours { get; } = effortInHours;

    public TimeTrackingHours EffortBilled { get; } = effortBilled;
    
    public TimeTrackingBillingMultiplier BillingMultiplier { get; } = billingMultiplier;

    public TimeTrackingEntryType Type { get; } = type;

    public string Description { get; } = description;
}