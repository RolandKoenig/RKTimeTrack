namespace RolandK.RemoteFileStorage;

// ReSharper disable once ClassNeverInstantiated.Global
public class FileDataStoreOptions
{
    public FileDataStoreType Type { get; set; }
    
    public string? FileSystemRootPath { get; set; }
    
    public string? S3ServiceUrl { get; set; }

    public string? S3AccessKey { get; set; }
    
    public string? S3SecretKey { get; set; }
    
    public string? S3BucketName { get; set; }
}