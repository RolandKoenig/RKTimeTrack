using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Migrations;

public class TimeTrackingDocumentMigrator
{
    public static readonly Version VERSION_1_0_0_0 = new(1, 0, 0,0);
    public static readonly Version VERSION_1_1_0_0 = new(1, 1, 0,0);
    public static readonly Version VERSION_1_2_0_0 = new(1, 2, 0,0);
    public static readonly Version CURRENT_VERSION = VERSION_1_2_0_0;
    
    public static TimeTrackingDocument Migrate(TimeTrackingDocument document)
    {
        var result = document;
        if (IsLowerVersion(result.Version, VERSION_1_1_0_0))
        {
             result = Migrate_to_1_1_0(result);
        }
        if (IsLowerVersion(result.Version, VERSION_1_2_0_0))
        {
            result = Migrate_to_1_2_0(result);
        }

        return result;
    }
    
    /// <summary>
    /// Document version 1.1.0.0 introduced <see cref="TimeTrackingEntry.EffortBilled"/>
    /// </summary>
    private static TimeTrackingDocument Migrate_to_1_1_0(TimeTrackingDocument document)
    {
        return new TimeTrackingDocument(
            VERSION_1_1_0_0.ToString(),
            document.Days.Select(actDay => new TimeTrackingDay(
                actDay.Date,
                actDay.Type,
                actDay.Entries.Select(actEntry => new TimeTrackingEntry(
                    new TimeTrackingTopicReference(
                        actEntry.Topic.Category,
                        actEntry.Topic.Name),
                    actEntry.EffortInHours,
                    actEntry.EffortBilled,
                    actEntry.Type switch
                    {
                        TimeTrackingEntryType.Training => 1.75,
                        TimeTrackingEntryType.OnCall => 0,
                        _ => 1.0
                    },
                    false,
                    actEntry.Type,
                    actEntry.Description)).ToList())).ToList());
    }
    
    /// <summary>
    /// Document version 1.1.0.0 introduced <see cref="TimeTrackingEntry.EffortBilled"/>
    /// </summary>
    private static TimeTrackingDocument Migrate_to_1_2_0(TimeTrackingDocument document)
    {
        return new TimeTrackingDocument(
            VERSION_1_2_0_0.ToString(),
            document.Days.Select(actDay => new TimeTrackingDay(
                actDay.Date,
                actDay.Type,
                actDay.Entries.Select(actEntry => new TimeTrackingEntry(
                    new TimeTrackingTopicReference(
                        actEntry.Topic.Category,
                        actEntry.Topic.Name),
                    actEntry.EffortInHours,
                    actEntry.EffortBilled,
                    actEntry.BillingMultiplier,
                    true,
                    actEntry.Type,
                    actEntry.Description)).ToList())).ToList());
    }
    
    private static bool IsLowerVersion(string versionString, Version reference)
    {
        if (!Version.TryParse(versionString, out var givenVersion))
        {
            throw new Exception($"Unable to parse version from runtime data during migration! (Value: '{versionString}'");
        }
        
        return givenVersion < reference;
    }
}