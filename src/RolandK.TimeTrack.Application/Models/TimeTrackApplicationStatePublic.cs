namespace RolandK.TimeTrack.Application.Models;

public record TimeTrackApplicationStatePublic(
    DateTimeOffset ServiceStartupTimestamp,
    DateTimeOffset? LastSuccessfulExport);