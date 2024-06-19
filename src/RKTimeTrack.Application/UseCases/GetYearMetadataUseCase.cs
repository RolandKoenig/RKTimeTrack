using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingYearMetadata,
    CommonErrors.ValidationError>;

public class GetYearMetadataUseCase
{
    public async Task<HandlerResult> GetYearMetadataAsync(GetYearMetadataRequest request, CancellationToken cancellationToken)
    {
        // Validate
        var validator = new GetYearMetadataRequest.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }
        
        // Create and return result
        var calendarWeekOfLastDay = GermanCalendarWeekUtil.GetCalendarWeek(new DateOnly(request.Year, 12, 31));
        return new TimeTrackingYearMetadata(
            maxWeekNumber: calendarWeekOfLastDay switch
            {
                53 => 53,
                _ => 52
            });
    }
}