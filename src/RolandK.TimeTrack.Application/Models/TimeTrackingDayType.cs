using System.Text.Json.Serialization;

namespace RolandK.TimeTrack.Application.Models;

[JsonConverter(typeof(JsonStringEnumConverter<TimeTrackingDayType>))]
public enum TimeTrackingDayType
{
    /// <summary>
    /// Normal working day.
    /// </summary>
    WorkingDay,
    
    /// <summary>
    /// Education for myself.
    /// </summary>
    OwnEducation,
    
    /// <summary>
    /// Some public holiday.
    /// </summary>
    PublicHoliday,
    
    /// <summary>
    /// Ill, not able to work.
    /// </summary>
    Ill,
    
    /// <summary>
    /// I give training for others.
    /// </summary>
    Training,
    
    /// <summary>
    /// Holiday
    /// </summary>
    Holiday,
    
    /// <summary>
    /// Compensatory time-off
    /// </summary>
    CompensatoryTimeOff,
    
    /// <summary>
    /// Weekend
    /// </summary>
    Weekend
}