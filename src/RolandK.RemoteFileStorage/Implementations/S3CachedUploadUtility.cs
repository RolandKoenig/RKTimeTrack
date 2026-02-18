using RolandK.RemoteFileStorage.Util;

namespace RolandK.RemoteFileStorage.Implementations;

/// <summary>
/// Very simple and not optimal solution.
/// We are writing all data into a MemoryStream first, afterwards we will do the upload to S3 using _uploadFunc
/// </summary>
class S3CachedUploadUtility : IUploadUtility, IDisposable
{
    private readonly Func<MemoryStream, CancellationToken, Task> _uploadFunc;
    
    private MemoryStream? _memoryStream;

    public Stream OutStream =>
        _memoryStream ?? throw new ObjectDisposedException(nameof(S3CachedUploadUtility));

    public S3CachedUploadUtility(Func<MemoryStream, CancellationToken, Task> uploadFunc)
    {
        _uploadFunc = uploadFunc;
        _memoryStream = ReusableMemoryStreams.Current.TakeMemoryStream();
    }
    
    public async Task CompleteUploadAsync(CancellationToken cancellationToken)
    {
        if (_memoryStream == null) { return; }

        _memoryStream.Seek(0, SeekOrigin.Begin);
        
        await _uploadFunc(_memoryStream, cancellationToken);
    }

    public void Dispose()
    {
        if (_memoryStream == null) { return; }
        
        ReusableMemoryStreams.Current.ReRegisterMemoryStream(_memoryStream);
        _memoryStream = null;
    }

    public ValueTask DisposeAsync()
    {
        this.Dispose();
        return ValueTask.CompletedTask;
    }
}