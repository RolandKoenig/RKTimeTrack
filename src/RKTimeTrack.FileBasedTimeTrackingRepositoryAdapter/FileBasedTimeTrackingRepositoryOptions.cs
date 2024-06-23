namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

public class FileBasedTimeTrackingRepositoryOptions
{
    public bool WriteIndentedJson { get; set; } = false;

    public string? PersistenceDirectory { get; set; } = null;
}