namespace RolandK.TimeTrack.Application.Models;

public class TimeTrackingTopicReference(string category, string name)
{
    public string Category { get; } = category;

    public string Name { get; } = name;
}