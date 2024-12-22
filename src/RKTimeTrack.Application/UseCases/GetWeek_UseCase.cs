using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingWeek,
    CommonErrors.ValidationError>;

public class GetWeek_UseCase(ITimeTrackingRepository timeTrackingRepository)
{
    public async Task<HandlerResult> GetWeekAsync(GetWeek_Request request, CancellationToken cancellationToken)
    {
        var validator = new GetWeek_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }
        
        var year = request.Year;
        var weekNumber = request.WeekNumber;
        CorrectLastWeekOfTheYear(ref year, ref weekNumber);
        
        var result = 
            await timeTrackingRepository.GetWeekAsync(year, weekNumber, cancellationToken);
        
        return result;
    }

    /// <summary>
    /// Update year if the given week 53 is actually in the following year.
    /// </summary>
    private static void CorrectLastWeekOfTheYear(ref int year, ref int weekNumber)
    {
        if (weekNumber != 53) { return; }
        
        var calendarWeekOfLastDay = GermanCalendarWeekUtil.GetCalendarWeek(new DateOnly(year, 12, 31));
        if (calendarWeekOfLastDay == 1)
        {
            year++;
            weekNumber = 1;
        }
    }
}