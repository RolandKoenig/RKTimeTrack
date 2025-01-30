using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.Tests.Models;

public class TimeTrackingBudgetTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1.25, 1)]
    [InlineData(1.3, 1)]
    [InlineData(1.4, 1)]
    [InlineData(5.6, 6)]
    [InlineData(5.7, 6)]
    [InlineData(6, 6)]
    public void Rounding(double input, double expectedOutput)
    {
        // Act
        var budget = new TimeTrackingBudget(input);
        
        // Assert
        Assert.Equal(expectedOutput, budget.Hours);
    }

    [Fact]
    public void Do_not_allow_negative_budgets()
    {
        // Act
        Action actAction = () => new TimeTrackingBudget(-1);
        
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(actAction);
    }

    [Fact]
    public void Implicit_conversion_from_double()
    {
        // Act
        TimeTrackingBudget myBudget = 5.7;
        
        // Assert
        Assert.Equal(6, myBudget.Hours);
    }
}