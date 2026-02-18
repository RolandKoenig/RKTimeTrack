using RolandK.RemoteFileStorage;

namespace RolandK.TimeTrack.StaticTopicRepositoryAdapter;

public class StaticTopicRepositoryOptions
{
    /// <summary>
    /// If true, generates test data for the topic repository.
    /// </summary>
    public bool GenerateTestData { get; set; }
    
    /// <summary>
    /// The relative file path within the <see cref="FileDataStore"/>
    /// </summary>
    public string? SourceFilePath { get; set; }
    
    public FileDataStoreOptions? FileDataStore { get; set; }
}