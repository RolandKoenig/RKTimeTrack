namespace RKTimeTrack.Application.Models;

public class TimeTrackingTopic(string category, string name, TimeTrackingBudget? budget = null)
{
    public string Category { get; } = category;

    public string Name { get; } = name;

    public TimeTrackingBudget? Budget { get; } = budget;
}