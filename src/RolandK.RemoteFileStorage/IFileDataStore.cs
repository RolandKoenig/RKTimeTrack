namespace RolandK.RemoteFileStorage;

public interface IFileDataStore
{
    public Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken);
    
    public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);
    
    public Task<IUploadUtility> UploadFileAsync(string filePath, CancellationToken cancellationToken);
}