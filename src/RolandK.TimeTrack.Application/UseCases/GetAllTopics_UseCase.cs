using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;

namespace RolandK.TimeTrack.Application.UseCases;

public class GetAllTopics_UseCase(ITopicRepository topicRepository)
{
    public async Task<IReadOnlyList<TimeTrackingTopic>> GetAllTopicsAsync(CancellationToken cancellationToken)
    {
        return await topicRepository.GetAllTopicsAsync(cancellationToken);
    }
}