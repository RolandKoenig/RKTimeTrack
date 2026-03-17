using System.Text.Json.Serialization;
using Light.GuardClauses;
using RolandK.TimeTrack.Application.Models.Json;

namespace RolandK.TimeTrack.Application.Models;

/// <summary>
/// Time spend for some work.
/// Value must be positive and be rounded to quarter hours.
/// </summary>
[JsonConverter(typeof(TimeTrackingBillingMultiplierJsonConverter))]
public readonly struct TimeTrackingBillingMultiplier(double multiplier)
{
    public double Multiplier { get; } = RoundHoursToQuarterHours(multiplier);

    private static double RoundHoursToQuarterHours(double hours)
    {
        hours.MustBeGreaterThanOrEqualTo(0);
        
        return Math.Round(hours * 4) / 4;
    }
    
    public static implicit operator TimeTrackingBillingMultiplier(double multiplier) => new (multiplier);
}