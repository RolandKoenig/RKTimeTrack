using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;

namespace RolandK.TimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingDay,
    CommonErrors.ValidationError>;

public class UpdateDay_UseCase(ITimeTrackingRepository timeTrackingRepository)
{
    public async Task<HandlerResult> UpdateDayAsync(UpdateDay_Request request, CancellationToken cancellationToken)
    {
        var validator = new UpdateDay_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }
        
        var result = await timeTrackingRepository.UpdateDayAsync(
            new TimeTrackingDay(request.Date, request.Type, request.Entries),
            cancellationToken);
        
        return result;
    }
}