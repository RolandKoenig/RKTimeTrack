using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.ExportAdapter.ExportModel;

namespace RolandK.TimeTrack.ExportAdapter;

internal static class TimeTrackingDataMapper
{
    public static IReadOnlyList<ExportDataRow> MapData(
        IReadOnlyList<TimeTrackingDay> days, 
        DateOnly today,
        int expectedRowCount)
    {
        var result = new List<ExportDataRow>(expectedRowCount);

        foreach (var actDay in days)
        {
            if (actDay.Date > today){ continue; }
            
            if (actDay.Entries.Count == 0)
            {
                result.Add(new ExportDataRow(
                    "Nichts",
                    "-",
                    actDay.Date.ToString("yyyy-MM-dd"),
                    MapDayType(actDay.Type),
                    "",
                    0,
                    0,
                    1,
                    "-"));
                continue;
            }
            
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
                    actEntry.BillingMultiplier.Multiplier,
                    actEntry.Description));
            }
        }
        
        return result;
    }

    private static string MapDayType(TimeTrackingDayType dayType)
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

    private static string MapEntryType(TimeTrackingEntryType entryType)
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