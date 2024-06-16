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
            Monday = new TimeTrackingDay(){ Date = dateMonday },
            Tuesday = new TimeTrackingDay() { Date = dateMonday.AddDays(1) },
            Wednesday = new TimeTrackingDay() { Date = dateMonday.AddDays(2) },
            Thursday = new TimeTrackingDay() { Date = dateMonday.AddDays(3) },
            Friday = new TimeTrackingDay() { Date = dateMonday.AddDays(4) },
            Saturday = new TimeTrackingDay() { Date = dateMonday.AddDays(5) },
            Sunday = new TimeTrackingDay() { Date = dateMonday.AddDays(6) }
        };

        return result;
    }
}