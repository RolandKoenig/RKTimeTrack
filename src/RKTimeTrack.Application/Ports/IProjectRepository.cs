using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Ports;

public interface IProjectRepository
{
    IReadOnlyCollection<Project> GetAllProjects();
}