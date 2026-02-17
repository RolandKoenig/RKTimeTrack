namespace RolandK.RemoteFileStorage.Implementations;

internal class FileSystemFileDataStore : IFileDataStore
{
    private readonly FileDataStoreOptions _options;
    
    public FileSystemFileDataStore(FileDataStoreOptions options)
    {
        _options = options;
    }
    
    public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = !string.IsNullOrEmpty(_options.FileSystemRootPath)
            ? Path.Combine(_options.FileSystemRootPath, filePath)
            : filePath;
        return Task.FromResult((Stream)File.OpenRead(fullPath));
    }
}