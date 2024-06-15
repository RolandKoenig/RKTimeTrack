using Light.GuardClauses;

namespace RKTimeTrack.Application.Models;

/// <summary>
/// A budget for a topic.
/// Value must be positiv and is rounded to full hours.
/// </summary>
public readonly struct TimeTrackingBudget(double hours)
{
    public double Hours { get; } = RoundHoursToQuarterHours(hours);

    private static double RoundHoursToQuarterHours(double hours)
    {
        hours.MustBeGreaterThanOrEqualTo(0);

        return Math.Round(hours, 0);
    }
    
    public static implicit operator TimeTrackingBudget(double hours) => new (hours);
}