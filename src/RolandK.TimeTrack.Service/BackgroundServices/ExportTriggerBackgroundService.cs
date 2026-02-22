using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Service.BackgroundServices;

public class ExportTriggerBackgroundService : BackgroundService
{
    private static readonly TimeSpan CYCLE_WAIT_TIME = TimeSpan.FromSeconds(30.0);
    
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeProvider _timeProvider;
    
    public ExportTriggerBackgroundService(
        ILogger<ExportTriggerBackgroundService> logger,
        IServiceProvider serviceProvider,
        TimeProvider timeProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _timeProvider = timeProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try { await Task.Delay(CYCLE_WAIT_TIME, _timeProvider, stoppingToken); }
            catch (OperationCanceledException) { continue; }

            await using var scope = _serviceProvider.CreateAsyncScope();
            var exportUseCase = scope.ServiceProvider.GetRequiredService<ExportTimeTrackingData_UseCase>();

            try
            {
                await exportUseCase.ExportTimeTrackingDataAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Nothing to do
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting time tracking data");
            }
        }
    }
}