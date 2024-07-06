namespace RKTimeTrack.Application.Services;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}