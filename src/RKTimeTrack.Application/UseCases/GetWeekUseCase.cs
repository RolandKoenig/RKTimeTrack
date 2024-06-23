using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingWeek,
    CommonErrors.ValidationError>;

public class GetWeekUseCase(ITimeTrackingRepository timeTrackingRepository)
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
        
        // Get the week from the repository
        var result = await timeTrackingRepository.GetWeekAsync(year, weekNumber, cancellationToken);
        
        return result;
    }
}