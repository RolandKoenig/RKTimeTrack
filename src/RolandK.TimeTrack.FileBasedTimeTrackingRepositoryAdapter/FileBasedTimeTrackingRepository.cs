using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter;

class FileBasedTimeTrackingRepository(TimeTrackingStore store) : ITimeTrackingRepository
{
    private readonly Lock _lockObject = new();
    private readonly TimeTrackingStore _store = store;
    
    // ReSharper disable once InconsistentlySynchronizedField
    public DateTimeOffset LastChangeTimestamp => _store.LastChangeTimestamp;
    
    public void RestoreFromDocument(TimeTrackingDocument document)
    {
        lock (_lockObject)
        {
            _store.RestoreFromDocument(document);
        }
    }

    public TimeTrackingDocument StoreToDocument()
    {
        lock (_lockObject)
        {
            return _store.StoreToDocument();
        }
    }

    public Task<IReadOnlyList<TimeTrackingDay>> GetAllDaysInAscendingOrderAsync(CancellationToken cancellationToken)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        return Task.FromResult(_store.GetAllDaysInAscendingOrderAsync());
    }

    public Task<TimeTrackingWeek> GetWeekAsync(int year, int weekNumber, CancellationToken cancellationToken)
    {
        lock (_lockObject)
        {
            return Task.FromResult(_store.GetOrCreateWeek(year, weekNumber));
        }
    }

    public Task<TimeTrackingDay> UpdateDayAsync(TimeTrackingDay day, CancellationToken cancellationToken)
    {
        lock (_lockObject)
        {
            return Task.FromResult(_store.AddOrUpdateDay(day));
        }
    }
}