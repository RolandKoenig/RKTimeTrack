using System.Text.Json;
using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.Application.Tests.Models;

public class TimeTrackingTopicTests
{
    [Fact]
    public void DeserializeModel_just_required_fields()
    {
        // Arrange
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
    
    [Fact]
    public void DeserializeModel_with_start_date()
    {
        // Arrange
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var serializedData = """
                             [{
                               "category": "Category1",
                               "name": "Name1",
                               "startDate": "2025-01-27"
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
        Assert.Equal(new DateOnly(2025, 1, 27), topic[0].StartDate);
        Assert.Null(topic[0].EndDate);
    }
    
    [Fact]
    public void DeserializeModel_with_end_date()
    {
        // Arrange
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var serializedData = """
                             [{
                               "category": "Category1",
                               "name": "Name1",
                               "endDate": "2025-01-27"
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
        Assert.Equal(new DateOnly(2025, 1, 27), topic[0].EndDate);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DeserializeModel_with_can_be_invoiced(bool canBeInvoiced)
    {
        // Arrange
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var serializedData = $$"""
                             [{
                               "category": "Category1",
                               "name": "Name1",
                               "canBeInvoiced": {{canBeInvoiced.ToString().ToLower()}}
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
        Assert.Equal(canBeInvoiced, topic[0].CanBeInvoiced);
        Assert.Null(topic[0].Budget);
        Assert.Null(topic[0].StartDate);
        Assert.Null(topic[0].EndDate);
    }

    [Fact]
    public void DeserializeModel_with_budget()
    {
        // Arrange
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var serializedData = """
                             [{
                               "category": "Category1",
                               "name": "Name1",
                               "budget": 12
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
        Assert.Equal(12, topic[0].Budget?.Hours);
        Assert.Null(topic[0].StartDate);
        Assert.Null(topic[0].EndDate);
    }
}