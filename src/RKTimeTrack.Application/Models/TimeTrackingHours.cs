using Light.GuardClauses;

namespace RKTimeTrack.Application.Models;

/// <summary>
/// Time spend for some work.
/// Value must be positiv and is rounded to quarter hours.
/// </summary>
public readonly struct TimeTrackingHours(double hours)
{
    public double Hours { get; } = RoundHoursToQuarterHours(hours);

    private static double RoundHoursToQuarterHours(double hours)
    {
        hours.MustBeGreaterThanOrEqualTo(0);
        
        return Math.Round(hours * 4) / 4;
    }
    
    public static implicit operator TimeTrackingHours(double hours) => new (hours);
}