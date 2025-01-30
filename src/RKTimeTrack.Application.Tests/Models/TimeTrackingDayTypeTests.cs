using Light.GuardClauses.FrameworkExtensions;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Tests.Models;

public class TimeTrackingDayTypeTests
{
    [Fact]
    public void GetExpectedWorkingHours_NoException_for_all_enum_values()
    {
        // Arrange
        var enumValues = Enum.GetValues<TimeTrackingDayType>();
        
        // Act
        enumValues.ForEach(x => x.GetExpectedWorkingHours());
    }
    
    [Fact]
    public void GetCalculationFactorForWorkingHours_NoException_for_all_enum_values()
    {
        // Arrange
        var enumValues = Enum.GetValues<TimeTrackingDayType>();
        
        // Act
        enumValues.ForEach(x => x.GetCalculationFactorForWorkingHours());
    }
}