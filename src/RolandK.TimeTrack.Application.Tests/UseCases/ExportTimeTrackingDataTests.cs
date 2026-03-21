using Microsoft.Extensions.Logging;
using NSubstitute;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;
using RolandK.TimeTrack.Application.State;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Application.Tests.UseCases;

[Trait("Category", "NoDependencies")]
public class ExportTimeTrackingDataTests
{
    [Fact]
    public async Task Export_does_nothing_after_service_start()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExportTimeTrackingData_UseCase>>();
        var repo = Substitute.For<ITimeTrackingRepository>();
        var exporter = Substitute.For<ITimeTrackingExporter>();
        var state = new TimeTrackApplicationState();
        
        var sut = new ExportTimeTrackingData_UseCase(logger, repo, exporter, state);

        // Act
        await sut.ExportTimeTrackingDataAsync(CancellationToken.None);

        // Assert
        await repo
            .DidNotReceive()
            .GetAllDaysInAscendingOrderAsync(Arg.Any<CancellationToken>());
        await exporter
            .DidNotReceive()
            .ExportAsync(
                Arg.Any<IReadOnlyList<TimeTrackingDay>>(), 
                Arg.Any<CancellationToken>());
        Assert.Equal(DateTimeOffset.MinValue, state.LastSuccessfulExport);
    }
    
    [Fact]
    public async Task Export_does_nothing_when_nothing_changed_since_last_export()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExportTimeTrackingData_UseCase>>();
        var repo = Substitute.For<ITimeTrackingRepository>();
        var exporter = Substitute.For<ITimeTrackingExporter>();

        var lastSuccessfulExport = new DateTimeOffset(2025, 01, 02, 12, 00, 00, TimeSpan.Zero);
        var state = new TimeTrackApplicationState
        {
            ServiceStartupTimestamp = new DateTimeOffset(2025, 01, 01, 10, 00, 00, TimeSpan.Zero),
            LastSuccessfulExport = lastSuccessfulExport
        };

        repo.LastChangeTimestamp.Returns(lastSuccessfulExport);

        var sut = new ExportTimeTrackingData_UseCase(logger, repo, exporter, state);

        // Act
        await sut.ExportTimeTrackingDataAsync(CancellationToken.None);

        // Assert
        await repo.DidNotReceive().GetAllDaysInAscendingOrderAsync(Arg.Any<CancellationToken>());
        await exporter.DidNotReceive().ExportAsync(Arg.Any<IReadOnlyList<TimeTrackingDay>>(), Arg.Any<CancellationToken>());
        Assert.Equal(lastSuccessfulExport, state.LastSuccessfulExport);
    }
    
    [Fact]
    public async Task Export_exports_after_first_change_after_startup()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExportTimeTrackingData_UseCase>>();
        var repo = Substitute.For<ITimeTrackingRepository>();
        var exporter = Substitute.For<ITimeTrackingExporter>();

        var startup = new DateTimeOffset(2025, 01, 01, 10, 00, 00, TimeSpan.Zero);
        var lastChange = startup.AddMinutes(2);

        var state = new TimeTrackApplicationState
        {
            ServiceStartupTimestamp = startup,
            LastSuccessfulExport = DateTimeOffset.MinValue
        };

        repo.LastChangeTimestamp.Returns(lastChange);

        var days = (IReadOnlyList<TimeTrackingDay>)
        [
            new TimeTrackingDay(
                new DateOnly(2025, 01, 01),
                TimeTrackingDayType.WorkingDay,
                Array.Empty<TimeTrackingEntry>())
        ];
        
        repo.GetAllDaysInAscendingOrderAsync(CancellationToken.None)
            .Returns(Task.FromResult(days));

        var sut = new ExportTimeTrackingData_UseCase(logger, repo, exporter, state);

        // Act
        await sut.ExportTimeTrackingDataAsync(CancellationToken.None);

        // Assert
        await repo.Received(1).GetAllDaysInAscendingOrderAsync(CancellationToken.None);
        await exporter.Received(1).ExportAsync(days, CancellationToken.None);
        Assert.Equal(lastChange, state.LastSuccessfulExport);
    }
    
    [Fact]
    public async Task Export_exports_after_changes()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ExportTimeTrackingData_UseCase>>();
        var repo = Substitute.For<ITimeTrackingRepository>();
        var exporter = Substitute.For<ITimeTrackingExporter>();

        var lastSuccessful = new DateTimeOffset(2025, 01, 03, 08, 00, 00, TimeSpan.Zero);
        var lastChange = lastSuccessful.AddSeconds(1);

        var state = new TimeTrackApplicationState
        {
            ServiceStartupTimestamp = new DateTimeOffset(2025, 01, 01, 10, 00, 00, TimeSpan.Zero),
            LastSuccessfulExport = lastSuccessful
        };

        repo.LastChangeTimestamp.Returns(lastChange);

        var days = (IReadOnlyList<TimeTrackingDay>)
        [
            new TimeTrackingDay(
                new DateOnly(2025, 01, 03),
                (TimeTrackingDayType)0,
                Array.Empty<TimeTrackingEntry>())
        ];
        
        repo
            .GetAllDaysInAscendingOrderAsync(CancellationToken.None)
            .Returns(Task.FromResult(days));

        var sut = new ExportTimeTrackingData_UseCase(logger, repo, exporter, state);

        // Act
        await sut.ExportTimeTrackingDataAsync(CancellationToken.None);

        // Assert
        await repo
            .Received(1)
            .GetAllDaysInAscendingOrderAsync(CancellationToken.None);
        await exporter
            .Received(1)
            .ExportAsync(days, CancellationToken.None);
        Assert.Equal(lastChange, state.LastSuccessfulExport);
    }
}