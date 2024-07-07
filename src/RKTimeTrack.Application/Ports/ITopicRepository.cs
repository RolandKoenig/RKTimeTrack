using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Ports;

public interface ITopicRepository
{
    Task<IReadOnlyList<TimeTrackingTopic>> GetAllTopicsAsync(CancellationToken cancellationToken);
}