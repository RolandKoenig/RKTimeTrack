using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RolandK.RemoteFileStorage;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter;

class TimeTrackingPersistenceService(
    ILogger<TimeTrackingPersistenceService> logger,
    IOptions<FileBasedTimeTrackingRepositoryOptions> options,
    FileBasedTimeTrackingRepository repository)
    : IHostedService
{
    private readonly ILogger _logger = logger;

    private Task? _persistenceLoopTask;
    private readonly CancellationTokenSource _persistenceLoopCancellationTokenSource = new();
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Restore last data
        // In case of any errors: We throw the exception up to cancel the starting process
        // of this service.
        if ((!options.Value.DisablePersistence) &&
            (options.Value.PersistenceFileDataStore != null))
        {
            var persistenceFileStore = FileDataStoreFactory.FromOptions(
                options.Value.PersistenceFileDataStore);

            var filePath = GetTargetFilePath();
            var fileExists = await persistenceFileStore.FileExistsAsync(
                filePath, cancellationToken);
            if (fileExists)
            {
                await using var inStream = await persistenceFileStore.DownloadFileAsync(
                    filePath, cancellationToken);
                
                var restoredDocument = await TimeTrackingDocument.LoadFromStreamAsync(inStream, cancellationToken);
                repository.RestoreFromDocument(restoredDocument);
            }
        }
        
        // Start persistence loop
        _persistenceLoopTask = RunPersistenceLoopAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        var persistenceLoopTask = _persistenceLoopTask;
        if (persistenceLoopTask == null)
        {
            return Task.CompletedTask;
        }
        
        _persistenceLoopCancellationTokenSource.Cancel();
        return persistenceLoopTask;
    }

    private async Task RunPersistenceLoopAsync()
    {
        IFileDataStore? persistenceFileStore = null;
        if ((!options.Value.DisablePersistence) &&
            (options.Value.PersistenceFileDataStore != null))
        {
            persistenceFileStore = FileDataStoreFactory.FromOptions(
                options.Value.PersistenceFileDataStore);
        }
        
        var lastPersist = DateTimeOffset.MinValue;
        while (!_persistenceLoopCancellationTokenSource.Token.IsCancellationRequested)
        {
            try { await Task.Delay(1000, _persistenceLoopCancellationTokenSource.Token); }
            catch (OperationCanceledException) { break; }

            if (persistenceFileStore == null)
            {
                continue;
            }

            // Check whether we have to persist or not
            if (lastPersist >= repository.LastChangeTimestamp)
            {
                continue;
            }
            
            // Persist data
            try
            {
                var timestampStartPersistence = DateTimeOffset.UtcNow;
                    
                await PersistDataAsync(persistenceFileStore, CancellationToken.None);

                lastPersist = timestampStartPersistence;
                _logger.LogInformation("Successfully persisted current data store");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while persisting data");
            }
        }
        
        // Run last persistence to ensure that all data is persisted
        if ((lastPersist >= repository.LastChangeTimestamp) &&
            (persistenceFileStore != null))
        {
            await PersistDataAsync(persistenceFileStore, CancellationToken.None);
        }
    }

     private async Task PersistDataAsync(
         IFileDataStore persistenceFileStore,
         CancellationToken cancellationToken)
     {
         var documentToWrite = repository.StoreToDocument();
         
         var targetFile = GetTargetFilePath();
         
         await using var uploadUtil = await persistenceFileStore.UploadFileAsync(
             targetFile, cancellationToken);
         await documentToWrite.WriteToStreamAsync(
             uploadUtil.OutStream, 
             options.Value.WriteIndentedJson, 
             cancellationToken);
         await uploadUtil.CompleteUploadAsync(cancellationToken);
     }

    private string GetTargetFilePath()
    {
        return "TimeTracking.json";
    }
}