namespace RolandK.TimeTrack.ExportAdapter.ExportModel;

public record ExportData(
    string Version,
    DateTimeOffset ExportZeitstempel,
    IReadOnlyList<ExportDataRow> Rows);