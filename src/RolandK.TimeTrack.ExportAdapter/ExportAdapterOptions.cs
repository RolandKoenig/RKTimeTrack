using RolandK.RemoteFileStorage;

namespace RolandK.TimeTrack.ExportAdapter;

public class ExportAdapterOptions
{
    public const string SECTION_NAME = "ExportAdapter";
    
    public bool WriteIndentedJson { get; set; } = false;
    
    public string? ExportFileName { get; set; }
    
    public FileDataStoreOptions? ExportFileDataStore { get; set; }
}