namespace RolandK.RemoteFileStorage.Implementations;

internal class FileSystemFileDataStore : IFileDataStore
{
    public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        return Task.FromResult((Stream)File.OpenRead(filePath));
    }
}