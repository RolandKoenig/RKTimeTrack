namespace RolandK.RemoteFileStorage.Implementations;

internal class FileSystemFileDataStore : IFileDataStore
{
    private readonly FileDataStoreOptions _options;
    
    public FileSystemFileDataStore(FileDataStoreOptions options)
    {
        _options = options;
    }

    public Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken)
    {
        return Task.FromResult(File.Exists(filePath));
    }

    public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = !string.IsNullOrEmpty(_options.FileSystemRootPath)
            ? Path.Combine(_options.FileSystemRootPath, filePath)
            : filePath;
        return Task.FromResult((Stream)File.OpenRead(fullPath));
    }

    public Task<IUploadUtility> UploadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = !string.IsNullOrEmpty(_options.FileSystemRootPath)
            ? Path.Combine(_options.FileSystemRootPath, filePath)
            : filePath;
        
        var directoryPath = Path.GetDirectoryName(fullPath);
        if ((!string.IsNullOrEmpty(directoryPath)) &&
            (!Directory.Exists(directoryPath)))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        return Task.FromResult(
            (IUploadUtility)new FakeUploadUtility(File.OpenWrite(fullPath)));
    }
}