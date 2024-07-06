using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    IReadOnlyCollection<TimeTrackingTopic>,
    CommonErrors.ValidationError>;

public class GetAllTopicsUseCase(ITopicRepository topicRepository)
{

    public async Task<HandlerResult> GetAllTopicsAsync(GetAllTopicsRequest request, CancellationToken cancellationToken)
    {
        // Validate
        var validator = new GetAllTopicsRequest.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        // Execute UseCase
        var result = topicRepository.GetAllTopics();
        return HandlerResult.FromT0(result);
    }
}