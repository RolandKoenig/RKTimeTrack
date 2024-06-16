using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Tests.Models;

public class TimeTrackingWeekTests
{

    [Fact]
    public void SerializeModel()
    {
        // Arrange
        var model = BuildModelForTest();
      
        // Act
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonOptions.WriteIndented = true;
        var serializedString = JsonSerializer.Serialize(model, jsonOptions);
        
        // Assert
        serializedString.Should().StartWith("""
                                            {
                                              "monday": {
                                                "date": "2024-06-17",
                                                "type": "WorkingDay",
                                                "entries": [
                                                  {
                                                    "topic": {
                                                      "category": "TestCategory",
                                                      "name": "TestName"
                                                    },
                                                    "effortInHours": 4,
                                                    "effortBilled": 3,
                                                    "description": "DummyDescription"
                                                  }
                                                ]
                                              }
                                            """);
    }

    [Fact]
    public void Serialize_and_deserialize()
    {
        // Arrange
        var model = BuildModelForTest();
      
        // Act
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        jsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        
        var serialization = JsonSerializer.Serialize(model, jsonOptions);
        var deserializedObject = JsonSerializer.Deserialize<TimeTrackingWeek>(serialization, jsonOptions);
        
        // Assert
        deserializedObject.Should().BeEquivalentTo(model);
    }

    /// <summary>
    /// Helper method to create a model to test serialization / deserialization.
    /// </summary>
    private static TimeTrackingWeek BuildModelForTest()
    {
        var random = new Random(100);
        var topic = new TimeTrackingTopicReference("TestCategory", "TestName");

        TimeTrackingDay CreateDayFunc(DateOnly date, TimeTrackingDayType dayType)
            => new(date, dayType, new[]
            {
                new TimeTrackingRow()
                {
                    Topic = topic,
                    EffortInHours = random.Next(4, 5),
                    EffortBilled = random.Next(3, 4),
                    Description = "DummyDescription"
                }
            });

        return new TimeTrackingWeek()
        {
            Monday = CreateDayFunc(new DateOnly(2024, 6, 17), TimeTrackingDayType.WorkingDay),
            Tuesday = CreateDayFunc(new DateOnly(2024, 6, 18), TimeTrackingDayType.WorkingDay),
            Wednesday = CreateDayFunc(new DateOnly(2024, 6, 19), TimeTrackingDayType.WorkingDay),
            Thursday = CreateDayFunc(new DateOnly(2024, 6, 20), TimeTrackingDayType.WorkingDay),
            Friday = CreateDayFunc(new DateOnly(2024, 6, 21), TimeTrackingDayType.WorkingDay),
            Saturday = CreateDayFunc(new DateOnly(2024, 6, 22), TimeTrackingDayType.Weekend),
            Sunday = CreateDayFunc(new DateOnly(2024, 6, 23), TimeTrackingDayType.Weekend)
        };
    }
}