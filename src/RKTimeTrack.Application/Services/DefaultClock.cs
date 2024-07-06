namespace RKTimeTrack.Application.Services;

public class DefaultClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}