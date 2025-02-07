using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.Application.Ports;

public interface ITimeTrackingRepository
{
    Task<IReadOnlyList<TimeTrackingDay>> GetAllDaysInAscendingOrderAsync(CancellationToken cancellationToken);
    
    Task<TimeTrackingWeek> GetWeekAsync(int year, int weekNumber, CancellationToken cancellationToken);

    Task<TimeTrackingDay> UpdateDayAsync(TimeTrackingDay day, CancellationToken cancellationToken);
}