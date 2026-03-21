using System.ComponentModel;

namespace RolandK.TimeTrack.ExportAdapter.ExportModel;

public record ExportDataRow(
    string Kategorie,
    string Thema,
    string Datum, // Format yyyy-mm-dd
    string TagTyp,
    string ZeilenTyp,
    double Zeitaufwand, // In hours
    double Abgerechnet, // In hours
    double BillingMultiplier,
    string Kommentar);