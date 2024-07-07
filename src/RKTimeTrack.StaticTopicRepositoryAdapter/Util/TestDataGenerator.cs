using RKTimeTrack.Application.Models;

namespace RKTimeTrack.StaticTopicRepositoryAdapter.Util;

static class TestDataGenerator
{
    public static IReadOnlyList<TimeTrackingTopic> CreateTestData()
    {
        var result = new List<TimeTrackingTopic>();
        
        result.Add(new TimeTrackingTopic("Category1", "Name1"));
        result.Add(new TimeTrackingTopic("Category1", "Name2"));
        result.Add(new TimeTrackingTopic("Category1", "Name3"));
        result.Add(new TimeTrackingTopic("Category1", "Name4"));
        result.Add(new TimeTrackingTopic("Category1", "Name5"));
        result.Add(new TimeTrackingTopic("Category1", "Name6 (With Budget)", new TimeTrackingBudget(30)));
            
        result.Add(new TimeTrackingTopic("Category2", "Name1"));
        result.Add(new TimeTrackingTopic("Category2", "Name2"));
        result.Add(new TimeTrackingTopic("Category2", "Name3"));
        result.Add(new TimeTrackingTopic("Category2", "Name4"));
        result.Add(new TimeTrackingTopic("Category2", "Name5"));
        result.Add(new TimeTrackingTopic("Category2", "Name6 (With Budget)", new TimeTrackingBudget(30)));
            
        return result;
    }
}