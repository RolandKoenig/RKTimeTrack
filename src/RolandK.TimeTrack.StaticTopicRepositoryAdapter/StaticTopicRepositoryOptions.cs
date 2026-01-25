using RolandK.RemoteFileStorage;

namespace RolandK.TimeTrack.StaticTopicRepositoryAdapter;

public class StaticTopicRepositoryOptions
{
    public bool GenerateTestData { get; set; }
    
    public string? SourceFilePath { get; set; }
    
    public FileDataStoreOptions? FileDataStore { get; set; }
}