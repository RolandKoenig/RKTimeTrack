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
        
        // Return dummy result
        var dateMonday = GermanCalendarWeekUtil.GetDateOfMonday(request.Year, request.WeekNumber);
        var result = new TimeTrackingWeek()
        {
            Monday = new TimeTrackingDay(dateMonday, TimeTrackingDayType.WorkingDay, []),
            Tuesday = new TimeTrackingDay(dateMonday.AddDays(1), TimeTrackingDayType.WorkingDay, []),
            Wednesday = new TimeTrackingDay(dateMonday.AddDays(2), TimeTrackingDayType.WorkingDay, []),
            Thursday = new TimeTrackingDay(dateMonday.AddDays(3), TimeTrackingDayType.WorkingDay, []),
            Friday = new TimeTrackingDay(dateMonday.AddDays(4), TimeTrackingDayType.WorkingDay, []),
            Saturday = new TimeTrackingDay(dateMonday.AddDays(5), TimeTrackingDayType.Weekend, []),
            Sunday = new TimeTrackingDay(dateMonday.AddDays(6), TimeTrackingDayType.Weekend, [])
        };

        return result;
    }
}