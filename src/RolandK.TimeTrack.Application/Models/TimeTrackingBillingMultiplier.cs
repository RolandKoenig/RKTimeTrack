using System.Text.Json.Serialization;
using RolandK.TimeTrack.Application.Models.Json;

namespace RolandK.TimeTrack.Application.Models;

/// <summary>
/// Multiplier for billing.
/// Value must be positive and be rounded to 0.1 steps.
/// </summary>
[JsonConverter(typeof(TimeTrackingBillingMultiplierJsonConverter))]
public readonly struct TimeTrackingBillingMultiplier(double multiplier)
{
    public static readonly TimeTrackingBillingMultiplier Default = new(1.0);
    
    public double Multiplier { get; } = RoundMultiplier(multiplier);

    private static double RoundMultiplier(double multiplier)
    {
        return Math.Round(multiplier, 1);
    }
    
    public static implicit operator TimeTrackingBillingMultiplier(double multiplier) => new (multiplier);
}