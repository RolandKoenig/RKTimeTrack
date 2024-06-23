using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    TimeTrackingDay,
    CommonErrors.ValidationError>;

public class UpdateDayUseCase(ITimeTrackingRepository timeTrackingRepository)
{
    public async Task<HandlerResult> UpdateDayAsync(UpdateDayRequest request, CancellationToken cancellationToken)
    {
        // Validate
        var validator = new UpdateDayRequest.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }
        
        // Update repository
        var result = await timeTrackingRepository.UpdateDayAsync(
            new TimeTrackingDay(request.Date, request.Type, request.Entries),
            cancellationToken);
        
        return result;
    }
}