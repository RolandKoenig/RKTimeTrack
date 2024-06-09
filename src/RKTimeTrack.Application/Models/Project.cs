namespace RKTimeTrack.Application.Models;

public class Project(string category, string topic, double? budget = null)
{
    public string Category { get; } = category;

    public string Topic { get; } = topic;

    public double? Budget { get; } = budget;
}