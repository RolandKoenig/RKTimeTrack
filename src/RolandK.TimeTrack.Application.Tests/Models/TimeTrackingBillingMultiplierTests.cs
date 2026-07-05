using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.Application.Tests.Models;

[Trait("Category", "NoDependencies")]
public class TimeTrackingBillingMultiplierTests
{
    [Theory]
    [InlineData(1.00, 1.00)]
    [InlineData(1.024, 1.00)]
    [InlineData(1.025, 1.00)]
    [InlineData(1.026, 1.05)]
    [InlineData(1.05, 1.05)]
    [InlineData(1.074, 1.05)]
    [InlineData(1.075, 1.05)]
    [InlineData(1.076, 1.1)]
    [InlineData(1.10, 1.10)]
    [InlineData(1.123, 1.10)]
    [InlineData(1.126, 1.15)]
    [InlineData(2.99, 3.00)]
    public void Rounding(double input, double expectedOutput)
    {
        // Act
        var billingMultiplier = new TimeTrackingBillingMultiplier(input);

        // Assert
        Assert.Equal(expectedOutput, billingMultiplier.Multiplier, precision: 10);
    }

    [Fact]
    public void Default_multiplier_is_one()
    {
        // Act
        var billingMultiplier = TimeTrackingBillingMultiplier.Default;

        // Assert
        Assert.Equal(1.0, billingMultiplier.Multiplier, precision: 10);
    }

    [Fact]
    public void Implicit_conversion_from_double()
    {
        // Act
        TimeTrackingBillingMultiplier billingMultiplier = 1.126;

        // Assert
        Assert.Equal(1.15, billingMultiplier.Multiplier, precision: 10);
    }
}