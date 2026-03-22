using NSubstitute;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;
using RolandK.TimeTrack.Application.UseCases;

namespace RolandK.TimeTrack.Application.Tests.UseCases;

[Trait("Category", "NoDependencies")]
public class SearchEntriesUseCaseTests
{
    private readonly ITimeTrackingRepository _timeTrackingRepository = Substitute.For<ITimeTrackingRepository>();
    private readonly ITopicRepository _topicRepository = Substitute.For<ITopicRepository>();
    private readonly SearchEntries_UseCase _useCase;

    public SearchEntriesUseCaseTests()
    {
        _useCase = new SearchEntries_UseCase(_timeTrackingRepository, _topicRepository);
    }

    [Fact]
    public async Task Search_with_no_filters_returns_empty()
    {
        // Act
        var result = await _useCase.SearchEntriesAsync(
            new SearchEntries_Request(""),
            CancellationToken.None);

        // Assert
        var entries = result.AsT0;
        Assert.Empty(entries);
    }
    
    [Fact]
    public async Task Search_with_Billed_filter_returns_only_matching_entries()
    {
        // Arrange
        var topic1 = new TimeTrackingTopic("Cat1", "Topic1", canBeInvoiced: true);
        var topic2 = new TimeTrackingTopic("Cat1", "Topic2", canBeInvoiced: false);
        _topicRepository.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(new List<TimeTrackingTopic> { topic1, topic2 });

        var entry1 = new TimeTrackingEntry(
            new TimeTrackingTopicReference("Cat1", "Topic1"), 
            new TimeTrackingHours(1), 
            billed: false,
            description: "Unbilled and CanBeInvoiced");
        var entry2 = new TimeTrackingEntry(
            new TimeTrackingTopicReference("Cat1", "Topic1"),
            new TimeTrackingHours(1),
            billed: true,
            description: "Billed and CanBeInvoiced");
        var entry3 = new TimeTrackingEntry(
            new TimeTrackingTopicReference("Cat1", "Topic2"),
            new TimeTrackingHours(1), 
            billed: false,
            description: "Unbilled and NOT CanBeInvoiced");
        var entry4 = new TimeTrackingEntry(
            new TimeTrackingTopicReference("Cat1", "Topic2"),
            new TimeTrackingHours(1), 
            billed: true,
            description: "Unbilled and NOT CanBeInvoiced");
        
        var day = new TimeTrackingDay(new DateOnly(2024, 1, 1), TimeTrackingDayType.WorkingDay, new[] { entry1, entry2, entry3, entry4 });
        _timeTrackingRepository.GetAllDaysInAscendingOrderAsync(Arg.Any<CancellationToken>())
            .Returns(new List<TimeTrackingDay> { day });

        // Act
        var result = await _useCase.SearchEntriesAsync(
            new SearchEntries_Request("", Billed: false, CanBeInvoiced: true),
            CancellationToken.None);

        // Assert
        var entries = result.AsT0;
        Assert.Single(entries);
        Assert.Equal("Unbilled and CanBeInvoiced", entries[0].Description);
    }

    [Fact]
    public async Task Search_with_only_Text_filter_returns_matching_entries()
    {
        // Arrange
        _topicRepository.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(new List<TimeTrackingTopic>());
        
        var entry1 = new TimeTrackingEntry(
            new TimeTrackingTopicReference("Cat1", "Topic1"), 
            new TimeTrackingHours(1),
            description: "Match");
        var entry2 = new TimeTrackingEntry(
            new TimeTrackingTopicReference("Cat1", "Topic1"), 
            new TimeTrackingHours(1), 
            description: "No");
        
        var day = new TimeTrackingDay(new DateOnly(2024, 1, 1), TimeTrackingDayType.WorkingDay, new[] { entry1, entry2 });
        _timeTrackingRepository.GetAllDaysInAscendingOrderAsync(Arg.Any<CancellationToken>())
            .Returns(new List<TimeTrackingDay> { day });

        // Act
        var result = await _useCase.SearchEntriesAsync(
            new SearchEntries_Request("Match"),
            CancellationToken.None);

        // Assert
        var entries = result.AsT0;
        Assert.Single(entries);
        Assert.Equal("Match", entries[0].Description);
    }
}
