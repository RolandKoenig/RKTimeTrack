using System.Text.Json;
using System.Text.Json.Serialization;
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
        Assert.StartsWith("""
                          {
                            "year": 2024,
                            "weekNumber": 25,
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
                                  "type": "Default",
                                  "description": "DummyDescription"
                                }
                              ]
                            }
                          """,
            serializedString);
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
        Assert.Equivalent(model, deserializedObject);
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
                new TimeTrackingEntry(
                    topic, 
                    effortInHours:random.Next(4,5), 
                    effortBilled:random.Next(3,4), 
                    description: "DummyDescription")
            });

        return new TimeTrackingWeek(
            year: 2024,
            25,
            monday: CreateDayFunc(new DateOnly(2024, 6, 17), TimeTrackingDayType.WorkingDay),
            tuesday: CreateDayFunc(new DateOnly(2024, 6, 18), TimeTrackingDayType.WorkingDay),
            wednesday: CreateDayFunc(new DateOnly(2024, 6, 19), TimeTrackingDayType.WorkingDay),
            thursday: CreateDayFunc(new DateOnly(2024, 6, 20), TimeTrackingDayType.WorkingDay),
            friday: CreateDayFunc(new DateOnly(2024, 6, 21), TimeTrackingDayType.WorkingDay),
            saturday: CreateDayFunc(new DateOnly(2024, 6, 22), TimeTrackingDayType.Weekend),
            sunday:CreateDayFunc(new DateOnly(2024, 6, 23), TimeTrackingDayType.Weekend)
        );
    }
}