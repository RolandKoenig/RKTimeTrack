namespace RolandK.RemoteFileStorage;

// ReSharper disable once ClassNeverInstantiated.Global
public class FileDataStoreOptions
{
    public string? ServiceUrl { get; set; }

    public string? AccessKey { get; set; }
    
    public string? SecretKey { get; set; }
    
    public string? BucketName { get; set; }
}