namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

public class FileBasedTimeTrackingRepositoryOptions
{
    public const string SECTION_NAME = "FileBasedTimeTrackingRepository";
    
    public bool WriteIndentedJson { get; set; } = false;
    
    public string? PersistenceDirectory { get; set; } = null;
}