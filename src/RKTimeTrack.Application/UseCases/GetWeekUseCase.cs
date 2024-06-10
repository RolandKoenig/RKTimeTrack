using RKTimeTrack.Application.Models;

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
        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid) { return new CommonErrors.ValidationError(result.Errors); }
        
        return new TimeTrackingWeek();
    }
}