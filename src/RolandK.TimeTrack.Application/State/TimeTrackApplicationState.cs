namespace RolandK.TimeTrack.Application.State;

public class TimeTrackApplicationState
{
    public DateTimeOffset ServiceStartupTimestamp { get; set; }
        = DateTimeOffset.UtcNow;
    
    public DateTimeOffset LastSuccessfulExport { get; set; } = DateTimeOffset.MinValue;
}