using RKTimeTrack.Application.Models;

namespace RKTimeTrack.StaticTopicRepositoryAdapter.Util;

static class TestDataGenerator
{
    public static IReadOnlyList<TimeTrackingTopic> CreateTestData()
    {
        var result = new List<TimeTrackingTopic>();

        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        
        result.Add(new TimeTrackingTopic("VisibleCategory-1", "1-ShouldBeVisible", startDate: currentDate.AddDays(-10), endDate: currentDate));
        result.Add(new TimeTrackingTopic("VisibleCategory-1", "2-ShouldBeVisible", startDate: currentDate, endDate: currentDate.AddDays(5)));
        result.Add(new TimeTrackingTopic("VisibleCategory-1", "3-ShouldBeVisible", startDate: currentDate.AddDays(-10), endDate: currentDate.AddDays(5)));
        result.Add(new TimeTrackingTopic("VisibleCategory-1", "ShouldBeHidden-1", startDate: currentDate.AddDays(-10), endDate: currentDate.AddDays(-5)));
        result.Add(new TimeTrackingTopic("VisibleCategory-1", "ShouldBeHidden-2", startDate: currentDate.AddDays(5), endDate: currentDate.AddDays(10)));
        result.Add(new TimeTrackingTopic("VisibleCategory-1", "4-ShouldBeVisible (With Budget)", true, new TimeTrackingBudget(30)));
            
        result.Add(new TimeTrackingTopic("VisibleCategory-2", "1-ShouldBeVisible", startDate: currentDate.AddDays(-10), endDate: currentDate));
        result.Add(new TimeTrackingTopic("VisibleCategory-2", "2-ShouldBeVisible", startDate: currentDate, endDate: currentDate.AddDays(5)));
        result.Add(new TimeTrackingTopic("VisibleCategory-2", "3-ShouldBeVisible", startDate: currentDate.AddDays(-10), endDate: currentDate.AddDays(5)));
        result.Add(new TimeTrackingTopic("VisibleCategory-2", "ShouldBeHidden-1", startDate: currentDate.AddDays(-10), endDate: currentDate.AddDays(-5)));
        result.Add(new TimeTrackingTopic("VisibleCategory-2", "ShouldBeHidden-2", startDate: currentDate.AddDays(5), endDate: currentDate.AddDays(10)));
        result.Add(new TimeTrackingTopic("VisibleCategory-2", "4-ShouldBeVisible (With Budget)", true, new TimeTrackingBudget(30)));
            
        result.Add(new TimeTrackingTopic("HiddenCategory-1", "ShouldBeHidden-1", startDate: currentDate.AddDays(-10), endDate: currentDate.AddDays(-5)));
        result.Add(new TimeTrackingTopic("HiddenCategory-1", "ShouldBeHidden-2", startDate: currentDate.AddDays(5), endDate: currentDate.AddDays(10)));
        
        return result;
    }
}