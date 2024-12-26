using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    IReadOnlyCollection<TimeTrackingTopic>,
    CommonErrors.ValidationError>;

public class GetAllTopics_UseCase(ITopicRepository topicRepository)
{

    public async Task<HandlerResult> GetAllTopicsAsync(GetAllTopics_Request request, CancellationToken cancellationToken)
    {
        var validator = new GetAllTopics_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }
        
        var result = 
            await topicRepository.GetAllTopicsAsync(cancellationToken);
        return HandlerResult.FromT0(result);
    }
}