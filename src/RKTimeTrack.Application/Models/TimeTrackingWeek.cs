namespace RKTimeTrack.Application.Models;

public class TimeTrackingWeek(
    TimeTrackingDay monday,
    TimeTrackingDay tuesday,
    TimeTrackingDay wednesday,
    TimeTrackingDay thursday,
    TimeTrackingDay friday,
    TimeTrackingDay saturday,
    TimeTrackingDay sunday)
{
   public TimeTrackingDay Monday { get; } = monday;

   public TimeTrackingDay Tuesday { get; } = tuesday;

   public TimeTrackingDay Wednesday { get; } = wednesday;

   public TimeTrackingDay Thursday { get; } = thursday;

   public TimeTrackingDay Friday { get; } = friday;

   public TimeTrackingDay Saturday { get; } = saturday;

   public TimeTrackingDay Sunday { get; } = sunday;
}