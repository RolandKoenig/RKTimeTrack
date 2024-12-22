using System.Net.Http.Json;
using FluentAssertions;
using NSubstitute;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Service.IntegrationTests.Util;
using Xunit;
using Xunit.Abstractions;

namespace RKTimeTrack.Service.IntegrationTests;

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
        topics.Should().NotBeNull();
        topics.Should().HaveCount(2);
        
        topics![0].Category.Should().Be("TestCategory");
        topics[0].Name.Should().Be("Topic1");
        topics[0].CanBeInvoiced.Should().BeFalse();
        topics[0].Budget.Should().BeNull();
        
        topics[1].Category.Should().Be("TestCategory");
        topics[1].Name.Should().Be("Topic2");
        topics[1].CanBeInvoiced.Should().BeTrue();
        topics[1].Budget.Should().NotBeNull();
        topics[1].Budget!.Value.Hours.Should().Be(200);
    }
}