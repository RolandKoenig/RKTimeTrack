using System.Net.Http.Json;
using NSubstitute;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Service.Tests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RolandK.TimeTrack.Service.Tests.IntegrationTests;

[Collection(nameof(TestEnvironmentCollection))]
public class TopicApiTests
{
    private readonly WebHostServerFixture _server;
    
    public TopicApiTests(
        WebHostServerFixture server,
        ITestOutputHelper testOutputHelper)
    {
        _server = server;
        _server.TestOutputHelper = testOutputHelper;
        _server.ProgramStartupMethod = Program.CreateApplication;
        
        _server.Reset();
    }

    [Fact]
    public async Task GetAllTopics()
    {
        // Arrange
        var httpClient = new HttpClient();
        httpClient.BaseAddress = _server.RootUri;

        _server.TopicRepositoryMock.GetAllTopicsAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((IReadOnlyList<TimeTrackingTopic>)[
                new TimeTrackingTopic("TestCategory", "Topic1"),
                new TimeTrackingTopic("TestCategory", "Topic2", true, 200)
            ]));

        // Act
        var topics = await httpClient.GetFromJsonAsync<IReadOnlyList<TimeTrackingTopic>>("api/ui/topics");

        // Assert
        Assert.NotNull(topics);
        Assert.Equal(2, topics?.Count);

        Assert.Equal("TestCategory", topics![0].Category);
        Assert.Equal("Topic1", topics[0].Name);
        Assert.False(topics[0].CanBeInvoiced);
        Assert.Null(topics[0].Budget);

        Assert.Equal("TestCategory", topics[1].Category);
        Assert.Equal("Topic2", topics[1].Name);
        Assert.True(topics[1].CanBeInvoiced);
        Assert.NotNull(topics[1].Budget);
        Assert.Equal(200, topics[1].Budget!.Value.Hours);
    }
}