using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingYearMetadata,
    CommonErrors.ValidationError>;

public class GetYearMetadata_UseCase
{
    public async Task<HandlerResult> GetYearMetadataAsync(GetYearMetadata_Request request, CancellationToken cancellationToken)
    {
        var validator = new GetYearMetadata_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }
        
        var calendarWeekOfLastDay = GermanCalendarWeekUtil.GetCalendarWeek(new DateOnly(request.Year, 12, 31), out _);
        return new TimeTrackingYearMetadata(
            maxWeekNumber: calendarWeekOfLastDay switch
            {
                53 => 53,
                _ => 52
            });
    }
}