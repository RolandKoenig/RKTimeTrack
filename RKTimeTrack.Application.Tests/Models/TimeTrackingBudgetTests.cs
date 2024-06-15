using FluentAssertions;
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
        budget.Hours.Should().Be(expectedOutput);
    }

    [Fact]
    public void Do_not_allow_negative_budgets()
    {
        // Act
        var actAction = () => new TimeTrackingBudget(-1);
        
        // Assert
        actAction.Should().Throw<ArgumentException>();
    }
}