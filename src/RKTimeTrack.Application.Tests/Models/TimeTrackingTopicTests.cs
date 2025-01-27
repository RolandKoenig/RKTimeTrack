using System.Text.Json;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Tests.Models;

public class TimeTrackingTopicTests
{
    [Fact]
    public void DeserializeModel_just_required_fields()
    {
        // Arrane
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var serializedData = """
                             [{
                               "category": "Category1",
                               "name": "Name1"
                             }]
                             """;
        
        // Act
        var topic = JsonSerializer.Deserialize<IReadOnlyList<TimeTrackingTopic>>(
            serializedData, jsonOptions);
        
        // Assert
        Assert.NotNull(topic);
        Assert.Single(topic);
        Assert.Equal("Category1", topic[0].Category);
        Assert.Equal("Name1", topic[0].Name);
        Assert.False(topic[0].CanBeInvoiced);
        Assert.Null(topic[0].Budget);
        Assert.Null(topic[0].StartDate);
        Assert.Null(topic[0].EndDate);
    }
}