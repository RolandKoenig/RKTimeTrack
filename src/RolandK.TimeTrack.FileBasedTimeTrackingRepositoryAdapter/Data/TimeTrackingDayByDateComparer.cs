using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

public class TimeTrackingDayByDateComparer : IComparer<TimeTrackingDay>
{
    public static readonly TimeTrackingDayByDateComparer Instance = new();
    
    public int Compare(TimeTrackingDay? x, TimeTrackingDay? y)
    {
        if (ReferenceEquals(x, y)) { return 0; }
        if (ReferenceEquals(null, y)) { return 1; }
        if (ReferenceEquals(null, x)) { return -1; }
        return x.Date.CompareTo(y.Date);
    }
}