using RolandK.RemoteFileStorage.Implementations;

namespace RolandK.RemoteFileStorage;

public static class FileDataStoreFactory
{
    public static IFileDataStore FromOptions(FileDataStoreOptions options)
    {
        return new S3FileDataStore(options);
    }
}