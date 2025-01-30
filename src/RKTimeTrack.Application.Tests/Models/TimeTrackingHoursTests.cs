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
        Assert.Equal(expectedOutput, budget);
    }
    
    [Fact]
    public void Do_not_allow_negative_hours()
    {
        // Act
        Action actAction = () => new TimeTrackingHours(-1);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(actAction);
    }
    
    [Fact]
    public void Implicit_conversion_from_double()
    {
        // Act
        TimeTrackingHours myBudget = 5.7;
        
        // Assert
        Assert.Equal(5.75, myBudget);
    }
}