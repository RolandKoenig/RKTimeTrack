using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.Application.Ports;

public interface ITimeTrackingExporter
{
    Task ExportAsync(IReadOnlyList<TimeTrackingDay> days, CancellationToken cancellationToken);
}