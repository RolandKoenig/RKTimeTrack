namespace RKTimeTrack.Application.Models;

public class TimeTrackingTopic(string category, string name, bool canBeInvoiced = false, TimeTrackingBudget? budget = null)
{
    public string Category { get; } = category;

    public string Name { get; } = name;

    public bool CanBeInvoiced { get; } = canBeInvoiced;

    public TimeTrackingBudget? Budget { get; } = budget;
}