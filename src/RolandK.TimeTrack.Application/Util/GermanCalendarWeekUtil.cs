using System.Globalization;
using Light.GuardClauses;

namespace RolandK.TimeTrack.Application.Util;

public static class GermanCalendarWeekUtil
{
    public const int CALENDAR_WEEK_MIN = 1;
    public const int CALENDAR_WEEK_MAX = 53;
    
    private static readonly CultureInfo CULTURE_INFO = new("de-DE");
    
    /// <summary>
    /// Gets the date for monday for given year and week number in germany.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="weekNumber">Week number, from 1 to 53</param>
    public static DateOnly GetDateOfMonday(int year, int weekNumber)
    {
        weekNumber.MustBeGreaterThanOrEqualTo(CALENDAR_WEEK_MIN);
        weekNumber.MustBeLessThanOrEqualTo(CALENDAR_WEEK_MAX);
        
        var firstDayOfYearInFirstCalendarWeek = new DateOnly(year, 1, 1);
        while (GetCalendarWeek(firstDayOfYearInFirstCalendarWeek, out _) > 1)
        {
            firstDayOfYearInFirstCalendarWeek = firstDayOfYearInFirstCalendarWeek.AddDays(1);
        }

        var anyDayFromRequestedWeek = weekNumber == 1
            ? firstDayOfYearInFirstCalendarWeek
            : firstDayOfYearInFirstCalendarWeek.AddDays(7 * (weekNumber - 1));
        
        var dayOfWeekNumber = (int)anyDayFromRequestedWeek.DayOfWeek;
        var result = dayOfWeekNumber == 1
            ? anyDayFromRequestedWeek
            : anyDayFromRequestedWeek.AddDays(1 - dayOfWeekNumber);

        return result;
    }
 
    /// <summary>
    /// Gets the calendar week for the given date in germany.
    /// </summary>
    public static int GetCalendarWeek(DateOnly date, out bool nextYear)
    {
        nextYear = false;
        
        // Method from
        // https://mycsharp.de/forum/threads/83188/datetime-nummer-der-kalenderwoche-ermitteln?page=1
        
        var dateTimeForCalculation = new DateTime(date, new TimeOnly(1, 0));
        
        // Get calendar week from the Calendar object.
        var calendarWeek = CULTURE_INFO.Calendar.GetWeekOfYear(
            dateTimeForCalculation,
            CULTURE_INFO.DateTimeFormat.CalendarWeekRule,
            CULTURE_INFO.DateTimeFormat.FirstDayOfWeek);

        // Check if a calendar week greater than 52
        // was determined and whether the calendar week of the date
        // results in a week 2: In this case, GetWeekOfYear has not calculated
        // the calendar week according to ISO 8601 (for example,
        // Monday takes the 31.12.2007 as WK 53 wrongly).
        // The calendar week is then set to 1 
        if (calendarWeek > 52)
        {
            dateTimeForCalculation = dateTimeForCalculation.AddDays(7);
            var testCalendarWeek =  CULTURE_INFO.Calendar.GetWeekOfYear(
                dateTimeForCalculation,
                CULTURE_INFO.DateTimeFormat.CalendarWeekRule,
                CULTURE_INFO.DateTimeFormat.FirstDayOfWeek);
            if (testCalendarWeek == 2)
            {
                calendarWeek = 1;
                nextYear = true;
            }
        }
        
        return calendarWeek;
    }
}