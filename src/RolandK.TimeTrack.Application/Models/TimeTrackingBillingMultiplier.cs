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
    public static readonly TimeTrackingBillingMultiplier Default = new(1.0);
    
    public double Multiplier { get; } = RoundHoursToQuarterHours(multiplier);

    public override string ToString() => $"{Multiplier:N2}x";
    
    private static double RoundHoursToQuarterHours(double hours)
    {
        return Math.Round(hours * 4) / 4;
    }
    
    public static implicit operator TimeTrackingBillingMultiplier(double multiplier) => new (multiplier);
}