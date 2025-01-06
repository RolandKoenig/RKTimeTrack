using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.Application.UseCases;

public class GetAllTopics_UseCase(ITopicRepository topicRepository)
{
    public async Task<IReadOnlyList<TimeTrackingTopic>> GetAllTopicsAsync(CancellationToken cancellationToken)
    {
        return await topicRepository.GetAllTopicsAsync(cancellationToken);
    }
}