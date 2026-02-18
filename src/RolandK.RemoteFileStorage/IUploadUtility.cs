namespace RolandK.RemoteFileStorage;

public interface IUploadUtility : IAsyncDisposable
{
    public Stream OutStream { get; }
    
    public Task CompleteUploadAsync(CancellationToken cancellationToken);
}