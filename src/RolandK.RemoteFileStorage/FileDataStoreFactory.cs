using RolandK.RemoteFileStorage.Implementations;

namespace RolandK.RemoteFileStorage;

public static class FileDataStoreFactory
{
    public static IFileDataStore FromOptions(FileDataStoreOptions options)
    {
        return options.Type switch
        {
            FileDataStoreType.FileSystem => new FileSystemFileDataStore(options),
            FileDataStoreType.S3 => new S3FileDataStore(options),
            _ => throw new ArgumentException($"Invalid file data store type: {options.Type}", nameof(options))
        };
    }
}