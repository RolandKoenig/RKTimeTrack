using FluentAssertions;
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
        var act = () => enumValues.ForEach(x => x.GetExpectedWorkingHours());
        
        // Assert
        act.Should().NotThrow();
    }
    
    [Fact]
    public void GetCalculationFactorForWorkingHours_NoException_for_all_enum_values()
    {
        // Arrange
        var enumValues = Enum.GetValues<TimeTrackingDayType>();
        
        // Act
        var act = () => enumValues.ForEach(x => x.GetCalculationFactorForWorkingHours());
        
        // Assert
        act.Should().NotThrow();
    }
}