using System.Text.Json.Serialization;

namespace RKTimeTrack.Application.Models;

[JsonConverter(typeof(JsonStringEnumConverter<TimeTrackingEntryType>))]
public enum TimeTrackingEntryType
{
    Default = 0,
    
    /// <summary>
    /// I give training for others.
    /// </summary>
    Training = 1
}