using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Migrations;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Tests.Migrations;

[Trait("Category", "NoDependencies")]
public class TimeTrackingDocumentMigratorTests
{
    [Theory]
    [InlineData(TimeTrackingEntryType.Default, 1.0)]
    [InlineData(TimeTrackingEntryType.Training, 1.75)]
    [InlineData(TimeTrackingEntryType.OnCall, 0.0)]
    public void Migrate_entries_of_different_row_types(TimeTrackingEntryType entryType, double expectedBillingMultiplier)
    {
        // Arrange
        var previousData = new TimeTrackingDocument(
            TimeTrackingDocumentMigrator.VERSION_1_0_0_0.ToString(),
            [
                new TimeTrackingDay(
                    new DateOnly(2022, 1, 1),
                    TimeTrackingDayType.WorkingDay,
                    [
                        new TimeTrackingEntry(
                            new TimeTrackingTopicReference("Category1", "Name1"),
                            2.0,
                            2.0,
                            0,
                            false,
                            entryType,
                            "Some dummy work")
                    ])
            ]);
        var previousDay = previousData.Days.First();
        var previousEntry = previousDay.Entries.First();
        
        // Act
        var migratedData = TimeTrackingDocumentMigrator.Migrate(previousData);
        
        // Assert
        Assert.NotNull(migratedData);
        Assert.NotEqual(migratedData, previousData);
        Assert.Equal(TimeTrackingDocumentMigrator.CURRENT_VERSION.ToString(), migratedData.Version);

        var migratedDay = migratedData.Days.First();
        var migratedEntry = migratedDay.Entries.First();
        Assert.Equal(expectedBillingMultiplier, migratedEntry.BillingMultiplier);
        Assert.True(migratedEntry.Billed);
        
        Assert.Equal(previousDay.Date, migratedDay.Date);
        Assert.Equal(previousDay.Type, migratedDay.Type);
        Assert.Equal(previousEntry.EffortInHours, migratedEntry.EffortInHours);
        Assert.Equal(previousEntry.EffortBilled, migratedEntry.EffortBilled);
        Assert.Equal(previousEntry.Description, migratedEntry.Description);
        Assert.Equal(previousEntry.Type, migratedEntry.Type);
        Assert.Equal(previousEntry.Topic.Category, migratedEntry.Topic.Category);
        Assert.Equal(previousEntry.Topic.Name, migratedEntry.Topic.Name);
    }
}