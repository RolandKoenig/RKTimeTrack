using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingWeek,
    CommonErrors.ValidationError>;

public class GetWeekUseCase
{
    public async Task<HandlerResult> GetWeekAsync(GetWeekRequest request, CancellationToken cancellationToken)
    {
        // Validate
        var validator = new GetWeekRequest.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        // Update year if the given week 53 is actually in the following year
        var year = request.Year;
        var weekNumber = request.WeekNumber;
        if (weekNumber == 53)
        {
            var calendarWeekOfLastDay = GermanCalendarWeekUtil.GetCalendarWeek(new DateOnly(year, 12, 31));
            if (calendarWeekOfLastDay == 1)
            {
                year++;
                weekNumber = 1;
            }
        }
        
        // Return dummy result
        var dateMonday = GermanCalendarWeekUtil.GetDateOfMonday(year, weekNumber);
        var result = new TimeTrackingWeek(
            year: year,
            weekNumber: weekNumber,
            monday: new TimeTrackingDay(dateMonday, TimeTrackingDayType.WorkingDay, []),
            tuesday: new TimeTrackingDay(dateMonday.AddDays(1), TimeTrackingDayType.WorkingDay, []),
            wednesday: new TimeTrackingDay(dateMonday.AddDays(2), TimeTrackingDayType.WorkingDay, []),
            thursday: new TimeTrackingDay(dateMonday.AddDays(3), TimeTrackingDayType.WorkingDay, []),
            friday: new TimeTrackingDay(dateMonday.AddDays(4), TimeTrackingDayType.WorkingDay, []),
            saturday: new TimeTrackingDay(dateMonday.AddDays(5), TimeTrackingDayType.Weekend, []),
            sunday:new TimeTrackingDay(dateMonday.AddDays(6), TimeTrackingDayType.Weekend, []));
        
        return result;
    }
}