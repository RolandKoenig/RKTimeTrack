using Microsoft.Extensions.Logging;
using RolandK.TimeTrack.Application.Ports;
using RolandK.TimeTrack.Application.State;

namespace RolandK.TimeTrack.Application.UseCases;

public class ExportTimeTrackingData_UseCase
{
    private readonly ILogger _logger;
    private readonly ITimeTrackingRepository _timeTrackingRepository;
    private readonly ITimeTrackingExporter _exporter;
    private readonly TimeTrackApplicationState _state;
    
    public ExportTimeTrackingData_UseCase(
        ILogger<ExportTimeTrackingData_UseCase> logger,
        ITimeTrackingRepository timeTrackingRepository,
        ITimeTrackingExporter exporter, 
        TimeTrackApplicationState state)
    {
        _logger = logger;
        _timeTrackingRepository = timeTrackingRepository;
        _exporter = exporter;
        _state = state;
    }

    public async Task ExportTimeTrackingDataAsync(CancellationToken cancellationToken)
    {
        // Check whether we have to export
        var lastChangeDate = _timeTrackingRepository.LastChangeTimestamp;
        var referenceTimestamp = _state.LastSuccessfulExport > DateTimeOffset.MinValue
            ? _state.LastSuccessfulExport
            : _state.ServiceStartupTimestamp;
        if (lastChangeDate <= referenceTimestamp) { return; }

        // Do the export
        _logger.LogInformation("Exporting time tracking data because last change at {LastExport}", lastChangeDate);
        
        var allDays = await _timeTrackingRepository.GetAllDaysInAscendingOrderAsync(cancellationToken);
        await _exporter.ExportAsync(allDays, cancellationToken);

        _logger.LogInformation("Successfully exported time tracking data for last change at {LastExport}", lastChangeDate);
        
        // Update last export timestamp on the application's state
        _state.LastSuccessfulExport = lastChangeDate;
    }
}