using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.Application.Ports;

public interface ITopicRepository
{
    Task<IReadOnlyList<TimeTrackingTopic>> GetAllTopicsAsync(CancellationToken cancellationToken);
}