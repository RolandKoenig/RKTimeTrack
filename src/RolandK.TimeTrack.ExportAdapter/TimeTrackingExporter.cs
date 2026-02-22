using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RolandK.RemoteFileStorage;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;
using RolandK.TimeTrack.ExportAdapter.ExportModel;

namespace RolandK.TimeTrack.ExportAdapter;

public class TimeTrackingExporter : ITimeTrackingExporter
{
    private readonly ILogger _logger;
    private readonly IOptions<ExportAdapterOptions> _options;
    
    public TimeTrackingExporter(
        ILogger<TimeTrackingExporter> logger,
        IOptions<ExportAdapterOptions> options)
    {
        _logger = logger;
        _options = options;
    }
    
    public async Task ExportAsync(
        IReadOnlyList<TimeTrackingDay> days, 
        CancellationToken cancellationToken)
    {
        if (!TryCreateFileDataStore(out var exportDataStore, out var exportFileName))
        {
            _logger.LogError("Unable to export time tracking data: Invalid FileStorage configuration!");
            return;
        }
        
        var expectedRowCount = CalculateExportRowCount(days);
        var exportRows = MapData(days, expectedRowCount);
        _logger.LogInformation("Start exporting {RowCount} rows time tracking data", exportRows.Count);

        await ExportRowsAsync(
            exportFileName,
            exportDataStore,
            exportRows,
            cancellationToken);
        
        _logger.LogInformation("Export of {RowCount} rows time tracking data completed successfully", exportRows.Count);
    }

    private bool TryCreateFileDataStore(out IFileDataStore exportDataStore, out string exportFileName)
    {
        if ((!string.IsNullOrEmpty(_options.Value.ExportFileName)) &&
            (_options.Value.ExportFileDataStore != null))
        {
            exportDataStore = FileDataStoreFactory.FromOptions(
                _options.Value.ExportFileDataStore);
            exportFileName = _options.Value.ExportFileName;
            return true;
        }
        else
        {
            exportDataStore = null!;
            exportFileName = string.Empty;
        }

        return false;
    }

    private int CalculateExportRowCount(IReadOnlyList<TimeTrackingDay> days)
    {
        var rowCount = 0;
        foreach (var actDay in days)
        {
            foreach (var actEntry in actDay.Entries)
            {
                rowCount++;
            }
        }
        return rowCount;
    }

    private async Task ExportRowsAsync(
        string exportFileName,
        IFileDataStore exportFileStorage,
        IReadOnlyList<ExportDataRow> rows,
        CancellationToken cancellationToken)
    {
        var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        serializerOptions.WriteIndented = _options.Value.WriteIndentedJson;
        
        await using var uploadUtil = await exportFileStorage.UploadFileAsync(
            exportFileName, cancellationToken);
        await JsonSerializer.SerializeAsync(
            uploadUtil.OutStream, 
            rows,
            serializerOptions, 
            cancellationToken);
        await uploadUtil.CompleteUploadAsync(cancellationToken);
        
        await Task.CompletedTask;
    }

    private IReadOnlyList<ExportDataRow> MapData(IReadOnlyList<TimeTrackingDay> days, int expectedRowCount)
    {
        var result = new List<ExportDataRow>(expectedRowCount);

        foreach (var actDay in days)
        {
            foreach (var actEntry in actDay.Entries)
            {
                result.Add(new ExportDataRow(
                    actEntry.Topic.Category,
                    actEntry.Topic.Name,
                    actDay.Date.ToString("yyyy-MM-dd"),
                    MapDayType(actDay.Type),
                    MapEntryType(actEntry.Type),
                    actEntry.EffortInHours.Hours,
                    actEntry.EffortBilled.Hours,
                    actEntry.Description));
            }
        }
        
        return result;
    }

    private string MapDayType(TimeTrackingDayType dayType)
    {
        return dayType switch
        {
            TimeTrackingDayType.OwnEducation => "BT",
            TimeTrackingDayType.PublicHoliday => "FT",
            TimeTrackingDayType.Ill => "KT",
            TimeTrackingDayType.Training => "ST",
            TimeTrackingDayType.TrainingPreparation => "SV",
            TimeTrackingDayType.Holiday => "UT",
            TimeTrackingDayType.CompensatoryTimeOff => "ZA",
            TimeTrackingDayType.Weekend => "WE",
            TimeTrackingDayType.WorkingDay => "",
            _ => throw new ArgumentOutOfRangeException(nameof(dayType), dayType, null),
        };
    }

    private string MapEntryType(TimeTrackingEntryType entryType)
    {
        return entryType switch
        {
            TimeTrackingEntryType.Training => "S",
            TimeTrackingEntryType.OnCall => "B",
            TimeTrackingEntryType.Default => "",
            _ => throw new ArgumentOutOfRangeException(nameof(entryType), entryType, null),
        };
    }
}