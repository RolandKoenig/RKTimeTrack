namespace RolandK.TimeTrack.Application.Models;

public class TimeTrackingWeek(
    int year,
    int weekNumber,
    TimeTrackingDay monday,
    TimeTrackingDay tuesday,
    TimeTrackingDay wednesday,
    TimeTrackingDay thursday,
    TimeTrackingDay friday,
    TimeTrackingDay saturday,
    TimeTrackingDay sunday)
{
    public int Year { get; } = year;

    public int WeekNumber { get; } = weekNumber;
   
   public TimeTrackingDay Monday { get; } = monday;

   public TimeTrackingDay Tuesday { get; } = tuesday;

   public TimeTrackingDay Wednesday { get; } = wednesday;

   public TimeTrackingDay Thursday { get; } = thursday;

   public TimeTrackingDay Friday { get; } = friday;

   public TimeTrackingDay Saturday { get; } = saturday;

   public TimeTrackingDay Sunday { get; } = sunday;
   
   public IEnumerable<TimeTrackingDay> GetAllDays() => 
   [
       this.Monday, 
       this.Tuesday, 
       this.Wednesday, 
       this.Thursday, 
       this.Friday, 
       this.Saturday, 
       this.Sunday
   ];
}