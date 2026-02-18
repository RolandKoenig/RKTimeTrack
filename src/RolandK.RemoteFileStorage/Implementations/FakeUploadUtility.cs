namespace RolandK.RemoteFileStorage.Implementations;

class FakeUploadUtility : IUploadUtility
{
    public Stream OutStream { get; private set; }

    public FakeUploadUtility(Stream fileStream)
    {
        this.OutStream = fileStream;
    }
        
    public Task CompleteUploadAsync(CancellationToken cancellationToken)
    {
        this.OutStream.Dispose();
            
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await this.OutStream.DisposeAsync();
    }
}