using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Ports;

public interface ITopicRepository
{
    IReadOnlyCollection<TimeTrackingTopic> GetAllProjects();
}