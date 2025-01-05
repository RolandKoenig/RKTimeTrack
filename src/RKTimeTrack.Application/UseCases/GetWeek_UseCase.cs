using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingWeek,
    CommonErrors.ValidationError>;

public class GetWeek_UseCase(ITimeTrackingRepository timeTrackingRepository, TimeProvider timeProvider)
{
    public async Task<HandlerResult> GetWeekAsync(GetWeek_Request request, CancellationToken cancellationToken)
    {
        var validator = new GetWeek_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        // Get current week
        int year;
        int weekNumber;
        if (request.IsWeekProvided())
        {
            year = request.Year!.Value;
            weekNumber = request.WeekNumber!.Value;
        }
        else
        {
            GetCurrentWeek(timeProvider, out year, out weekNumber);
        }
        
        // We need to correct some edge cases
        CorrectLastWeekOfTheYear(ref year, ref weekNumber);
        
        // Get the week from the repository
        var result = 
            await timeTrackingRepository.GetWeekAsync(year, weekNumber, cancellationToken);
        
        return result;
    }

    /// <summary>
    /// Get current week (year + weekNumber) from given <see cref="TimeProvider"/>
    /// </summary>
    private static void GetCurrentWeek(TimeProvider timeProvider, out int year, out int weekNumber)
    {
        var now = timeProvider.GetUtcNow();
        weekNumber = GermanCalendarWeekUtil.GetCalendarWeek(
            DateOnly.FromDateTime(now.DateTime),
            out var nextYear);
        year = nextYear ? now.Year + 1 : now.Year;
    }

    /// <summary>
    /// Update year if the given week 53 is actually in the following year.
    /// </summary>
    private static void CorrectLastWeekOfTheYear(ref int year, ref int weekNumber)
    {
        if (weekNumber != 53) { return; }
        
        var calendarWeekOfLastDay = GermanCalendarWeekUtil.GetCalendarWeek(new DateOnly(year, 12, 31), out _);
        if (calendarWeekOfLastDay == 1)
        {
            year++;
            weekNumber = 1;
        }
    }
}