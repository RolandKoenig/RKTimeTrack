namespace RKTimeTrack.Application.Models;

public static class TimeTrackingDayTypeExtensions
{
    /// <summary>
    /// Gets the expected working hours for the given <see cref="TimeTrackingDayType" />.
    /// Working hours higher than the given value result in overtime.
    /// </summary>
    public static double GetExpectedWorkingHours(this TimeTrackingDayType dayType) => dayType switch
    {
        TimeTrackingDayType.Holiday => 0,
        TimeTrackingDayType.Ill => 0,
        TimeTrackingDayType.OwnEducation => 0,
        TimeTrackingDayType.PublicHoliday => 0,
        TimeTrackingDayType.Training => CommonConstants.DEFAULT_EXPECTED_WORKING_HOURS,
        TimeTrackingDayType.Weekend => 0,
        TimeTrackingDayType.WorkingDay => CommonConstants.DEFAULT_EXPECTED_WORKING_HOURS,
        _ => throw new ArgumentOutOfRangeException(nameof(dayType), dayType, null)
    };

    /// <summary>
    /// Gets a multiplicator for working hours for the given <see cref="TimeTrackingDayType"/>.
    /// A value other than 1 is for cases where more hours are actually calculated to be done for a given amount of working hours.
    /// The initial example are training sessions. For training, you bill only the training hours. Not the preparation.
    /// This factor ensures that the preparation time goes into time calculation correctly.
    /// </summary>
    public static double GetCalculationFactorForWorkingHours(this TimeTrackingDayType dayType) => dayType switch
    {
        TimeTrackingDayType.Holiday => 1,
        TimeTrackingDayType.Ill => 1,
        TimeTrackingDayType.OwnEducation => 1,
        TimeTrackingDayType.PublicHoliday => 1,
        TimeTrackingDayType.Training => CommonConstants.WORKING_HOUR_CALCULATION_FACTOR_FOR_TRAINING,
        TimeTrackingDayType.Weekend => 1,
        TimeTrackingDayType.WorkingDay => 1,
        _ => throw new ArgumentOutOfRangeException(nameof(dayType), dayType, null)
    };
}