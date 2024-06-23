using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

class FileBasedTimeTrackingRepository : ITimeTrackingRepository
{
    private readonly object _lockObject = new();
    private readonly TimeTrackingStore _store = new();
    
    private DateTimeOffset _lastChangeTimestamp = DateTimeOffset.MinValue;

    public DateTimeOffset LastChangeTimestamp => _lastChangeTimestamp;
    
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
            var result = Task.FromResult(_store.AddOrUpdateDay(day));
            _lastChangeTimestamp = DateTimeOffset.UtcNow;
            
            return result;
        }
    }
}