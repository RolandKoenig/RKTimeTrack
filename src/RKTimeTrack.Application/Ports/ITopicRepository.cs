using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Ports;

public interface ITopicRepository
{
    Task<IReadOnlyCollection<TimeTrackingTopic>> GetAllTopicsAsync(CancellationToken cancellationToken);
}