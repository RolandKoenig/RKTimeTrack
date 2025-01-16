using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter;

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
        // Restore previous data
        var targetFilepath = GetTargetFilePath();
        if (File.Exists(targetFilepath))
        {
            await using var inStream = File.OpenRead(targetFilepath);
            var restoredDocument = await TimeTrackingDocument.LoadFromStreamAsync(inStream, cancellationToken);

            repository.RestoreFromDocument(restoredDocument);
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
        var lastPersist = DateTimeOffset.MinValue;
        while (!_persistenceLoopCancellationTokenSource.Token.IsCancellationRequested)
        {
            try { await Task.Delay(1000, _persistenceLoopCancellationTokenSource.Token); }
            catch (TaskCanceledException) { break; }

            // Check whether we have to persist or not
            if (lastPersist >= repository.LastChangeTimestamp)
            {
                continue;
            }
            
            // Persist data
            try
            {
                var timestampStartPersistence = DateTimeOffset.UtcNow;
                    
                await PersistDataAsync(CancellationToken.None);

                lastPersist = timestampStartPersistence;
                _logger.LogInformation("Successfully persisted current data store");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while persisting data");
            }
        }
        
        // Run last persistence to ensure that all data is persisted
        await PersistDataAsync(CancellationToken.None);
    }

     private async Task PersistDataAsync(CancellationToken cancellationToken)
     {
         var targetFile = GetTargetFilePath();

         var documentToWrite = repository.StoreToDocument();
         
         await using var outStream = File.Create(targetFile);
         await documentToWrite.WriteToStreamAsync(outStream, options.Value.WriteIndentedJson, cancellationToken);
    }

    private string GetTargetFilePath()
    {
        var outDirectory = !string.IsNullOrEmpty(options.Value.PersistenceDirectory)
            ? options.Value.PersistenceDirectory
            : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var outFile = Path.Combine(outDirectory, "TimeTracking.json");
        return outFile;
    }
}