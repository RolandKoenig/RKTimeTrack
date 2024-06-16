using FluentAssertions;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Tests.Models;

public class TimeTrackingHoursTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1.25, 1.25)]
    [InlineData(1.3, 1.25)]
    [InlineData(1.4, 1.5)]
    [InlineData(5.6, 5.5)]
    [InlineData(5.7, 5.75)]
    [InlineData(6, 6)]
    public void Rounding(double input, double expectedOutput)
    {
        // Act
        var budget = new TimeTrackingHours(input);
        
        // Assert
        budget.Hours.Should().Be(expectedOutput);
    }
    
    [Fact]
    public void Do_not_allow_negative_hours()
    {
        // Act
        var actAction = () => new TimeTrackingHours(-1);
        
        // Assert
        actAction.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void Implicit_conversion_from_double()
    {
        // Act
        TimeTrackingHours myBudget = 5.7;
        
        // Assert
        myBudget.Hours.Should().Be(5.75);
    }
}