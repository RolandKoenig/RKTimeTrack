namespace RolandK.RemoteFileStorage;

public interface IFileDataStore
{
    public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);
}